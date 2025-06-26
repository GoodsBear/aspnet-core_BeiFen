using Educational.Classgrade;
using Educational.Dto.ClassRooms;
using Educational.Enmu;
using Educational.Organization;
using Educational.Positions;
using Educational.RBAC;
using Educational.StaffTypes;
using Educational.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Staffs
{
    [ApiExplorerSettings(GroupName = "成员")]
    public class StaffServices : ApplicationService, IStaffServices
    {
        private readonly IConfiguration configuration;
        private readonly IRepository<StaffInfo,Guid> basicRepository;
        private readonly IRepository<Position, Guid> positionRep;
        private readonly IRepository<Role, Guid> roleRep;
        private readonly IRepository<StaffTypeInfo, Guid> typeRep;
        private readonly IRepository<OrganizationModel, Guid> organizationRepository;

        public StaffServices(IConfiguration configuration, IRepository<StaffInfo, Guid> basicRepository,IRepository<Position, Guid> positionRep,IRepository<Role,Guid> roleRep,IRepository<StaffTypeInfo,Guid> typeRep,IRepository<OrganizationModel, Guid> organizationRepository)
        {
            this.configuration = configuration;
            this.basicRepository = basicRepository;
            this.positionRep = positionRep;
            this.roleRep = roleRep;
            this.typeRep = typeRep;
            this.organizationRepository = organizationRepository;
        }
        /// <summary>
        /// 获取成员列表下拉框
        /// </summary>
        /// <returns>返回成员列表下拉框</returns>
        public async Task<ApiResult<List<StaffSelectDto>>> GetStaffAsync()
        {
            try
            {
                var queryable = await basicRepository.GetListAsync();
                var results = ObjectMapper.Map<List<StaffInfo>, List<StaffSelectDto>>(queryable);
                return ApiResult<List<StaffSelectDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "成员下拉框获取失败");
                throw;
            }
        }
        /// <summary>分页查询员工信息</summary>
        /// <param name="search">查询条件</param>
        /// <returns>分页结果，包含员工信息</returns>
        [HttpGet]
        //[Authorize]
        public async Task<ApiResult<ApiPaging<List<ShowStaffDTO>>>> GetStaffListAsync([FromQuery]SearchStaffDTO search)
        {
            try
            {
                // 获取员工数据源
                var staffinfo = await basicRepository.GetQueryableAsync();

                // 按员工姓名模糊查询
                if (!search.StaffName.IsNullOrEmpty())
                {
                    staffinfo = staffinfo.Where(s => s.StaffName.Contains(search.StaffName));
                }

                // 按员工状态筛选
                if (search.Status != null)
                {
                    staffinfo = staffinfo.Where(s => s.Status == search.Status);
                }

                // 分页处理
                var stafflist = staffinfo.Page(search.PageIndex, search.PageSize);

                // 映射成前端显示用的 DTO 列表
                var resultList = ObjectMapper.Map<List<StaffInfo>, List<ShowStaffDTO>>(stafflist.ToList());

                foreach (var item in resultList)
                {
                    item.Position = (await positionRep.GetAsync(item.PositionId)).PositionName;
                    item.Role = (await roleRep.GetAsync(item.RoleId)).RoleName;
                    item.StaffType = (await typeRep.GetAsync(item.StaffTypeId)).StaffTypeName;
                }

                // 封装分页数据
                ApiPaging<List<ShowStaffDTO>> paging = new ApiPaging<List<ShowStaffDTO>>
                {
                    TotleCount = stafflist.Count(),
                    TotlePage = (int)Math.Ceiling(stafflist.Count() * 1.0 / search.PageSize),
                    Data = resultList
                };

                // 返回成功结果
                return ApiResult<ApiPaging<List<ShowStaffDTO>>>.Success(ResultCode.Ok, paging);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "员工信息获取失败");
                throw; // 暂时抛出，可拓展成统一异常处理
            }
        }

        /// <summary>
        /// 批量设置用户所属机构
        /// </summary>
        /// <param name="userIds">要设置的用户ID集合</param>
        /// <param name="organizationIds">要设置的机构ID集合</param>
        /// <returns>操作结果</returns>
        public async Task<ApiResult> UpdateStaffOranization([FromQuery]Guid[] Ids, Guid[] organizationIds)
        {
            try
            {
                // 1. 验证输入参数
                if (Ids == null || Ids.Length == 0)
                    return ApiResult.Fail(ResultCode.Fail, "请选择要设置的用户");

                if (organizationIds == null || organizationIds.Length == 0)
                    return ApiResult.Fail(ResultCode.Fail, "请选择要设置的机构");

                // 2. 获取所有机构名称
                var orgNames = await (await organizationRepository.GetQueryableAsync())
                    .Where(s => organizationIds.Contains(s.Id))
                    .Select(s => s.Name) 
                    .ToListAsync();

                foreach (var item in Ids)
                {
                    var staffinfo = await basicRepository.FirstOrDefaultAsync(u => u.Id == item);
                    if (staffinfo.Organization != null)
                    {
                        // 清空原有机构信息
                        staffinfo.Organization = string.Empty;
                        // 设置新的机构信息
                        staffinfo.Organization = string.Join(",", orgNames);
                        await basicRepository.UpdateAsync(staffinfo);
                    }
                    staffinfo.Organization = string.Join(",", orgNames);
                    await basicRepository.UpdateAsync(staffinfo);
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail(ResultCode.Fail, "批量设置机构失败");
            }
        }

        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="addorUpdStaffDTO">前端传入的员工数据 DTO</param>
        /// <returns>返回封装的 ApiResult 包含新增员工信息</returns>
        [HttpPost]
        public async Task<ApiResult<ShowStaffDTO>> AddStaff(AddorUpdStaffDTO addorUpdStaffDTO)
        {
            try
            {
                // 检查员工名称是否重复
                var isExist = await basicRepository.AnyAsync(x => x.StaffName == addorUpdStaffDTO.StaffName);
                if (isExist)
                {
                    return ApiResult<ShowStaffDTO>.Fail(ResultCode.Fail, "员工名称已存在");
                }
                addorUpdStaffDTO.StaffPassword = Sha256Hash(addorUpdStaffDTO.StaffPassword);
                // 将前端传入的 AddorUpdStaffDTO 映射成实体 StaffInfo，用于数据库操作
                var staffinfo = ObjectMapper.Map<AddorUpdStaffDTO, StaffInfo>(addorUpdStaffDTO);

                // 将新员工数据插入数据库
                await basicRepository.InsertAsync(staffinfo);

                // 将插入后的实体对象映射成返回给前端的 ShowStaffDTO
                var showstaffinfo = ObjectMapper.Map<StaffInfo, ShowStaffDTO>(staffinfo);

                // 封装返回结果，状态码 OK，附带员工信息
                return ApiResult<ShowStaffDTO>.Success(ResultCode.Ok, showstaffinfo);
            }
            catch (Exception)
            {
                // 捕获异常（可以考虑在此处记录日志）
                throw; // 暂时继续抛出异常，可以扩展为日志记录或自定义错误返回
            }
        }

        /// <summary>
        /// 编辑员工信息（不修改密码）
        /// </summary>
        /// <param name="addorUpdStaffDTO">前端传入的员工数据 DTO</param>
        /// <returns>返回封装的 ApiResult 包含编辑后的员工信息</returns>
        [HttpPut]
        public async Task<ApiResult<ShowStaffDTO>> UpdateStaff(Guid staffId, AddorUpdStaffDTO addorUpdStaffDTO)
        {
            try
            {
                // 根据员工ID查出原始数据
                var staffinfo = await basicRepository.FindAsync(staffId);
                // 将 DTO 映射成数据库实体
                ObjectMapper.Map(addorUpdStaffDTO, staffinfo);

                // 更新员工信息到数据库
                await basicRepository.UpdateAsync(staffinfo);

                // 映射成返回给前端的 DTO
                var showstaffinfo = ObjectMapper.Map<StaffInfo, ShowStaffDTO>(staffinfo);

                // 返回结果
                return ApiResult<ShowStaffDTO>.Success(ResultCode.Ok, showstaffinfo);
            }
            catch (Exception)
            {
                // 获取异常（可以考虑在此处记录日志）
                throw; // 暂时直接抛出异常，可以扩展为日志记录或自定义错误返回
            }
        }

        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <param name="staffId">员工 ID</param>
        /// <returns>返回封装的 ApiResult 表示操作结果</returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteStaff([FromQuery]Guid[] Ids)
        {
                try
                {
                    // 1. 参数验证
                    if (Ids == null || Ids.Length == 0)
                    {
                        return ApiResult.Fail(ResultCode.Fail, "请选择要删除的员工");
                    }

                    // 2. 批量查询员工信息
                    var staffList = await basicRepository.GetListAsync(u => Ids.Contains(u.Id));

                    // 3. 验证是否全部找到
                    if (staffList.Count != Ids.Length)
                    {
                        var foundIds = staffList.Select(s => s.Id).ToArray();
                        var missingIds = Ids.Except(foundIds).ToArray();
                        return ApiResult.Fail(ResultCode.Fail, $"以下员工不存在：{string.Join(",", missingIds)}");
                    }

                    // 4. 批量删除
                    await basicRepository.DeleteManyAsync(staffList);

                    return ApiResult.Success(ResultCode.Ok);
                }
                catch (Exception)
            {
                // 获取异常（可以考虑在此处记录日志）
                throw; // 暂时直接抛出异常，可以扩展为日志记录或自定义错误返回
            }
        }

        /// <summary>
        /// 修改员工状态
        /// </summary>
        /// <param name="staffId">员工 ID</param>
        /// <param name="status">要修改成的员工状态</param>
        /// <returns>返回封装的 ApiResult 表示操作结果</returns>
        [HttpPut]
        public async Task<ApiResult> UpdateStaffStatus(Guid[] Ids, StaffStatus status)
        {
            try
            {
                // 1. 参数验证
                if (Ids == null || Ids.Length == 0)
                {
                    return ApiResult.Fail(ResultCode.Fail, "请选择要修改的员工");
                }

                // 2. 批量查询员工信息
                var staffList = await basicRepository.GetListAsync(u => Ids.Contains(u.Id));

                // 3. 验证是否全部找到
                if (staffList.Count != Ids.Length)
                {
                    var foundIds = staffList.Select(s => s.Id).ToArray();
                    var missingIds = Ids.Except(foundIds).ToArray();
                    return ApiResult.Fail(ResultCode.Fail, $"以下员工不存在：{string.Join(",", missingIds)}");
                }

                // 4. 批量更新状态
                foreach (var staff in staffList)
                {
                    staff.Status = status;
                    await basicRepository.UpdateAsync(staff);
                }

                // 返回操作成功
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception)
            {
                // 获取异常（可以考虑在此处记录日志）
                throw; // 暂时直接抛出异常，可以扩展为日志记录或自定义错误返回
            }
        }

        /// <summary>
        /// 修改员工密码
        /// </summary>
        /// <param name="staffId">员工 ID</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>返回封装的 ApiResult，状态码表示操作结果</returns>
        [HttpPut]
        public async Task<ApiResult> UpdateStaffPassword(Guid staffId, string newPassword)
        {
            try
            {
                // 查询员工信息
                var staffinfo = await basicRepository.FindAsync(staffId);
                if (staffinfo == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "员工不存在");
                }

                // 加密密码并更新
                staffinfo.StaffPassword = Sha256Hash(newPassword);
                await basicRepository.UpdateAsync(staffinfo);

                // 返回成功
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception)
            {
                // 获取异常（可以考虑在此处记录日志）
                throw; // 暂时直接抛出异常，可以扩展为日志记录或自定义错误返回
            }
        }

        /// <summary>
        /// 导出员工列表
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns>返回导出结果</returns>
        [HttpGet]
        public async Task<ApiResult<ExportResult>> GetExportStaffList()
        {
            // 获取员工数据源
            var staffinfo = await basicRepository.GetQueryableAsync();
            // 映射成DTO
            var staffdto=ObjectMapper.Map<List<StaffInfo>,List<ShowStaffDTO>>(staffinfo.ToList());
            // 调用导出帮助类生成 Excel
            var fileBytes = ExcelExporter.Export(staffdto, "员工信息", "员工信息表");
            // 返回导出结果
            return ApiResult<ExportResult>.Success(ResultCode.Ok, new ExportResult
            {
                FileName = $"员工信息_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
                FileContent = fileBytes
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDTO">登录请求 DTO，包含账户和密码</param>
        /// <returns>返回登录结果</returns>
        [HttpPost]
        public async Task<ApiResult<LoginReturnDTO>> Login([FromQuery] LoginDTO loginDTO)
        {
            try
            {
                // 查找用户
                var staff = await basicRepository.FirstOrDefaultAsync(x => x.StaffAccount == loginDTO.StaffAccount);

                // 如果用户不存在，返回失败
                if (staff == null)
                {
                    return ApiResult<LoginReturnDTO>.Fail(ResultCode.Fail, "用户不存在");
                }

                // 使用 SHA256 加密输入的密码并与数据库中的密码进行比对
                string hashedPassword = Sha256Hash(loginDTO.StaffPassword); // 调用加密方法

                if (staff.StaffPassword != hashedPassword)
                {
                    return ApiResult<LoginReturnDTO>.Fail(ResultCode.Fail, "密码错误");
                }

                var returndto=ObjectMapper.Map<StaffInfo, LoginReturnDTO>(staff);
                returndto.Token = GenerateJwtToken(staff);

                // 登录成功，返回用户信息
                return ApiResult<LoginReturnDTO>.Success(ResultCode.Ok, returndto);
            }
            catch (Exception ex)
            {
                // 异常处理（记录日志等）
                Logger.LogError(ex, "登录时发生异常");
                throw; // 继续抛出异常
            }
        }

        /// <summary>
        /// 生成JWT令牌=
        /// </summary>
        /// <param name="staff">员工实体</param>
        /// <returns>JWT令牌字符串</returns>
        private string GenerateJwtToken(StaffInfo staff)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecurityKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            // 添加声明
            new Claim("Id", staff.Id.ToString()),
            new Claim("StaffName", staff.StaffName ?? string.Empty),
            new Claim("StaffAccount", staff.StaffAccount ?? string.Empty),
            new Claim("StaffPhone", staff.StaffPhone ?? string.Empty),
            new Claim("StaffGender", staff.StaffGender ?? string.Empty),
            new Claim("StaffTypeId", staff.StaffTypeId.ToString()),
            new Claim("PositionId", staff.PositionId.ToString()),
            new Claim("RoleId", staff.RoleId.ToString()),
            new Claim("Organization", staff.Organization ?? string.Empty),
            new Claim("StaffStatus", staff.Status.ToString()),
            new Claim("PhotoUrl", staff.PhotoUrl ?? string.Empty),
            new Claim("Birthday", staff.Birthday?.ToString("yyyy-MM-dd") ?? string.Empty),

        }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpirationInMinutes"])),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// 使用 SHA256 加密密码
        /// </summary>
        /// <param name="input">原始密码</param>
        /// <returns>SHA256 加密后的密码</returns>

        private string Sha256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder stringBuilder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
