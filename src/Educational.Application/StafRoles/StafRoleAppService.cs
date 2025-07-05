using Educational.RBAC;
using Educational.Staffs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<StafRoleAppService> logger;
        public StafRoleAppService(IRepository<StaffRole, Guid> staffRoleRepository, ILogger<StafRoleAppService> logger, IRepository<StaffInfo, Guid> staffRepository, IRepository<Role, Guid> roleRepository)
        {
            this.staffRoleRepository = staffRoleRepository;
            this.logger = logger;
            this.staffRepository = staffRepository;
            this.roleRepository = roleRepository;
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

    }
}
