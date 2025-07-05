using Castle.Core.Logging;
using Educational.Positions;
using Educational.RBAC;
using Educational.Staffs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.StafRoles
{
    [ApiExplorerSettings(GroupName = "成员分配角色")]
    public class StafRoleAppService : ApplicationService, IStafRoleAppService
    {
        private readonly IRepository<StaffRole, Guid> staffRoleRepository;
        private readonly IRepository<StaffInfo, Guid> staffRepository;
        private readonly IRepository<Role, Guid> roleRepository;
        private readonly IRepository<Position> ppositionrepository;
        private readonly ILogger<StafRoleAppService> logger;
        public StafRoleAppService(IRepository<StaffRole, Guid> staffRoleRepository, ILogger<StafRoleAppService> logger, IRepository<StaffInfo, Guid> staffRepository, IRepository<Role, Guid> roleRepository,IRepository<Position> ppositionrepository)
        {
            this.staffRoleRepository = staffRoleRepository;
            this.logger = logger;
            this.staffRepository = staffRepository;
            this.roleRepository = roleRepository;
            this.ppositionrepository = ppositionrepository;
        }
        /// <summary>
        /// 给角色分配用户
        /// </summary>
        /// <param name="input">角色分配成员dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult> AddRoleStaff(RoleStaffDto input)
        {
            try
            {
                //校验角色是否存在
                var role = await roleRepository.FindAsync(input.RoleId);
                if (role == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "角色不存在");
                }
                // 查找该角色已有成员（获取所有已分配该角色的用户ID）
                var oldUsers = await staffRoleRepository.GetListAsync(x => x.RoleId == input.RoleId);
                var oldStaffIds = oldUsers.Select(x => x.StaffId);

                //只添加还没有的成员（排除已存在的成员）
                var newStaffIds = input.StaffIds.Where(id => !oldStaffIds.Contains(id)).ToList();
                if (newStaffIds.Any())
                {
                    // 构建需要新增的StaffRole对象集合
                    var createRoles = newStaffIds.Select(userid => new StaffRole
                    {
                        RoleId = input.RoleId,
                        StaffId = userid
                    }).ToList();
                    await staffRoleRepository.InsertManyAsync(createRoles);
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError($"添加角色用户失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 用户分配角色
        /// </summary>
        /// <param name="input">用户分配角色dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult> AddStaffRole(StaffRoleDto input)
        {
            try
            {
                // 校验用户是否存在
                var user = await staffRepository.FindAsync(input.StaffId);
                if (user == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "用户不存在");
                }
                // 删除原有角色分配
                var oldRoles = await staffRoleRepository.GetListAsync(x => x.StaffId == input.StaffId);
                if (oldRoles.Any())
                {
                    await staffRoleRepository.DeleteManyAsync(oldRoles);
                }
                // 批量插入新角色
                var createRoles = input.RoleIds.Select(roleid => new StaffRole
                {
                    StaffId = input.StaffId,
                    RoleId = roleid
                }).ToList();
                await staffRoleRepository.InsertManyAsync(createRoles);

                return ApiResult.Success(ResultCode.Ok);

            }
            catch (Exception ex)
            {
                logger.LogError($"添加用户角色失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 根据角色ID批量移除指定用户ID集合的成员
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="staffIds">要移除的用户ID集合</param>
        /// <returns>操作结果</returns>
        public async Task<ApiResult> RemoveStaffFromRole(Guid roleId, List<Guid> staffIds)
        {
            try
            {
                // 校验角色是否存在
                var role = await roleRepository.FindAsync(roleId);
                if (role == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "角色不存在");
                }
                // 查找该角色下需要移除的成员
                var toRemove = await staffRoleRepository.GetListAsync(x => x.RoleId == roleId && staffIds.Contains(x.StaffId));
                if (toRemove.Any())
                {
                    // 批量删除这些成员
                    await staffRoleRepository.DeleteManyAsync(toRemove);
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError($"批量移除角色成员失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 通过角色ID获取该角色下的用户列表 (带分页)
        /// </summary>
        /// <param name="searchStaffRoleDto">查询参数DTO，包含RoleId、PageIndex和PageSize</param>
        /// <returns>包含角色下用户列表的DTO (带分页信息)</returns>
        [HttpGet]
        [Route("GetRoleIdsToShowStaffList")] // 路由可以简化，因为参数都在FromQuery中
        public async Task<ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>> GetRoleIdToShowStaffList(SearchStaffRoleDto searchStaffRoleDto)
        {
            try
            {
                // 1. 验证角色是否存在
                var role = await roleRepository.GetAsync(searchStaffRoleDto.RoleId);
                if (role == null)
                {
                    // 修正泛型类型以匹配方法签名：ApiPaging<List<GetRoleIdToShowStaffDto>>
                    return ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>.Fail(ResultCode.Fail, "角色不存在");
                }

                // 2. 从 StaffRole 中间表获取所有关联到该角色的 StaffId
                var staffRoles = await staffRoleRepository.GetListAsync(sr => sr.RoleId == searchStaffRoleDto.RoleId);

                // 3. 提取所有 StaffId
                var staffIds = staffRoles.Select(sr => sr.StaffId).ToList();

                // 4. 根据 StaffId 查询 StaffInfo 获取员工详细信息
                var allStaffInfosInRole = new List<StaffInfo>();
                if (staffIds.Any()) // 避免空列表查询数据库
                {
                    allStaffInfosInRole = await staffRepository.GetListAsync(si => staffIds.Contains(si.Id));
                }

                // 5. 计算分页信息并应用内存分页
                var totleCount = allStaffInfosInRole.Count;
                var pagedStaffInfos = allStaffInfosInRole
                                        .Skip((searchStaffRoleDto.PageIndex - 1) * searchStaffRoleDto.PageSize)
                                        .Take(searchStaffRoleDto.PageSize)
                                        .ToList();

                // 6. 组装数据到 List<StaffDto>
                var pagedStaffDtos = new List<StaffDto>();
                foreach (var staffInfo in pagedStaffInfos)
                {
                    var position = await ppositionrepository.FirstOrDefaultAsync(p => p.Id == staffInfo.PositionId);

                    pagedStaffDtos.Add(new StaffDto
                    {
                        StaffId = staffInfo.Id,
                        StaffName = staffInfo.StaffName,
                        StaffTel = staffInfo.StaffPhone,
                        StaffJob = position?.PositionName
                    });
                }

                // 7. 封装到 GetRoleIdToShowStaffDto 实例
                var getRoleIdToShowStaffDtoInstance = new GetRoleIdToShowStaffDto
                {
                    RoleId = searchStaffRoleDto.RoleId, // 将前台传过来的RoleId给传入列表dto的角色id字段里面
                    children = pagedStaffDtos // 将分页后的 StaffDto 列表赋值给 children
                };

                // 8. 封装到 ApiPaging<List<GetRoleIdToShowStaffDto>>
                // 重要的是：ApiPaging 的 Data 是 List<GetRoleIdToShowStaffDto>，所以我们将单个实例放入一个列表中
                var apiPagingResult = new ApiPaging<List<GetRoleIdToShowStaffDto>>
                {
                    TotleCount = totleCount,
                    TotlePage = (int)Math.Ceiling((double)totleCount / searchStaffRoleDto.PageSize),
                    Data = new List<GetRoleIdToShowStaffDto> { getRoleIdToShowStaffDtoInstance } // 将 GetRoleIdToShowStaffDto 实例放入一个 List 中
                };

                // 如果没有找到任何关联的员工ID，但业务逻辑需要返回特定消息
                if (!staffRoles.Any() && totleCount == 0) // 双重确认，确保没有实际数据
                {
                    return ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>.Fail(ResultCode.Fail, "该角色下没有成员");
                }

                // 返回成功结果
                // 注意：您的原始代码中 Success 方法调用是 `Success(ResultCode.Ok,apiPagingResult)`，
                // 但通常 Success 方法的第一个参数是 Data，第二个才是 Message。
                // 我将它修改为更常见的 `Success(apiPagingResult, "操作成功")` 或者直接 `Success(apiPagingResult)`。
                // 如果您希望 Success 也带 ResultCode.Ok，请确保 ApiResult.Success 有这个重载。
                return ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>.Success(ResultCode.Ok, apiPagingResult);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "获取用户列表出错");
                return ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>.Fail(ResultCode.Fail, "获取用户列表时发生内部错误：" + ex.Message);
            }
        }

        /// <summary>
        /// 通过角色ID获取所有不属于该角色的用户列表 (带分页)
        /// </summary>
        /// <param name="searchStaffRoleDto">查询参数DTO，包含RoleId、PageIndex和PageSize</param>
        /// <returns>包含所有不属于该角色的用户列表的DTO (带分页信息)</returns>
        [HttpGet]
        [Route("GetStaffsNotInRole")] // 路由可以简化，因为参数都在FromQuery中
        public async Task<ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>> GetStaffsNotInRole(
            [FromQuery] SearchStaffRoleDto searchStaffRoleDto) // 使用 SearchStaffRoleDto 接收包含 RoleId 和分页参数
        {
            try
            {
                // 1. 验证角色是否存在
                var role = await roleRepository.GetAsync(searchStaffRoleDto.RoleId);
                if (role == null)
                {
                    // 修正泛型类型以匹配方法签名：ApiPaging<List<GetRoleIdToShowStaffDto>>
                    return ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>.Fail(ResultCode.Fail, "角色不存在");
                }

                // 2. 获取所有员工的ID
                var allStaffs = await staffRepository.GetListAsync();
                var allStaffIds = allStaffs.Select(s => s.Id).ToList();

                // 3. 获取属于该角色的员工ID
                var staffsInRole = await staffRoleRepository.GetListAsync(sr => sr.RoleId == searchStaffRoleDto.RoleId);
                var staffIdsInRole = staffsInRole.Select(sr => sr.StaffId).ToList();

                // 4. 计算不属于该角色的员工ID (AllStaffIds - StaffIdsInRole)
                var staffIdsNotInRole = allStaffIds.Except(staffIdsInRole).ToList();

                // 5. 根据不属于该角色的员工ID查询 StaffInfo 获取员工详细信息
                var staffsNotInRoleInfo = new List<StaffInfo>();
                if (staffIdsNotInRole.Any())
                {
                    staffsNotInRoleInfo = await staffRepository.GetListAsync(si => staffIdsNotInRole.Contains(si.Id));
                }

                // 6. 计算分页信息并应用内存分页
                var totleCount = staffsNotInRoleInfo.Count;
                var pagedStaffInfos = staffsNotInRoleInfo
                                        .Skip((searchStaffRoleDto.PageIndex - 1) * searchStaffRoleDto.PageSize)
                                        .Take(searchStaffRoleDto.PageSize)
                                        .ToList();

                // 7. 组装数据到 List<StaffDto>
                var pagedStaffDtos = new List<StaffDto>();
                foreach (var staffInfo in pagedStaffInfos)
                {
                    var position = await ppositionrepository.FirstOrDefaultAsync(p => p.Id == staffInfo.PositionId);

                    pagedStaffDtos.Add(new StaffDto
                    {
                        StaffId = staffInfo.Id,
                        StaffName = staffInfo.StaffName,
                        StaffTel = staffInfo.StaffPhone,
                        StaffJob = position?.PositionName
                    });
                }

                // 8. 封装到 GetRoleIdToShowStaffDto 实例
                var getRoleIdToShowStaffDtoInstance = new GetRoleIdToShowStaffDto
                {
                    RoleId = searchStaffRoleDto.RoleId, // 将前台传过来的RoleId给传入列表dto的角色id字段里面
                    children = pagedStaffDtos // 将分页后的 StaffDto 列表赋值给 children
                };

                // 9. 封装到 ApiPaging<List<GetRoleIdToShowStaffDto>>
                // 重要的是：ApiPaging 的 Data 是 List<GetRoleIdToShowStaffDto>，所以我们将单个实例放入一个列表中
                var apiPagingResult = new ApiPaging<List<GetRoleIdToShowStaffDto>>
                {
                    TotleCount = totleCount,
                    TotlePage = (int)Math.Ceiling((double)totleCount / searchStaffRoleDto.PageSize),
                    Data = new List<GetRoleIdToShowStaffDto> { getRoleIdToShowStaffDtoInstance } // 将 GetRoleIdToShowStaffDto 实例放入一个 List 中
                };

                // 如果没有找到任何不属于该角色的员工，但业务逻辑需要返回特定消息
                if (!staffIdsNotInRole.Any() && totleCount == 0) // 双重确认，确保没有实际数据
                {
                    // 在这种情况下，ApiPaging<List<GetRoleIdToShowStaffDto>> 的 Data 也会是包含一个 GetRoleIdToShowStaffDto，
                    // 且该 GetRoleIdToShowStaffDto 的 children 列表为空。
                    return ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>.Fail(ResultCode.Fail, "没有不属于该角色的成员");
                }


                return ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>.Success(ResultCode.Ok,apiPagingResult);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "获取不属于该角色的用户列表出错");
                return ApiResult<ApiPaging<List<GetRoleIdToShowStaffDto>>>.Fail(ResultCode.Fail, "获取不属于该角色的用户列表时发生内部错误：" + ex.Message);
            }
        }
    }
}
