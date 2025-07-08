using Educational.Enmu;
using Castle.Components.DictionaryAdapter;
using Abp.Authorization;
using Educational.Enmu;
using Educational.Organization;
using Educational.Positions;
using Educational.RBAC;
using Educational.SalarySetting;
using Educational.Shared.Models;
using Educational.StaffTypes;
using Educational.Tools;
using Lazy.Captcha.Core;
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
        private readonly IRepository<StaffInfo, Guid> basicRepository;
        private readonly IRepository<Position, Guid> positionRep;
        private readonly IRepository<Role, Guid> roleRep;
        private readonly IRepository<StaffTypeInfo, Guid> typeRep;
        private readonly IRepository<OrganizationModel, Guid> organizationRepository;
        private readonly IRepository<SalarySettingModel, Guid> salarySettingRepository;
        private readonly IRepository<StaffRole, Guid> staffrolerepository; //用户角色中间表
        private readonly IRepository<RolePermission, Guid> rolepermissionrepository; //角色权限中间表
        private readonly IRepository<Permissions, Guid> permissionrepository; //权限表
        private readonly IRepository<Educational.Menu.Menu, Guid> menuRepository; //菜单
        private readonly IRepository<ClassHourFeeSetting, Guid> classHourFeeSettingRepository;
        ILogger<StaffServices> logger;
        private readonly ICaptcha captcha;

        public StaffServices(IConfiguration configuration, IRepository<StaffInfo, Guid> basicRepository, IRepository<Position, Guid> positionRep, IRepository<Role, Guid> roleRep, IRepository<StaffTypeInfo, Guid> typeRep, IRepository<OrganizationModel, Guid> organizationRepository, IRepository<SalarySettingModel, Guid> salarySettingRepository, IRepository<StaffRole, Guid> staffrolerepository, IRepository<RolePermission, Guid> rolepermissionrepository, IRepository<Permissions, Guid> permissionrepository, IRepository<Menu.Menu, Guid> menuRepository, IRepository<ClassHourFeeSetting, Guid> classHourFeeSettingRepository, ILogger<StaffServices> logger, ICaptcha captcha)
        {
            this.configuration = configuration;
            this.basicRepository = basicRepository;
            this.positionRep = positionRep;
            this.roleRep = roleRep;
            this.typeRep = typeRep;
            this.organizationRepository = organizationRepository;
            this.salarySettingRepository = salarySettingRepository;
            this.staffrolerepository = staffrolerepository;
            this.rolepermissionrepository = rolepermissionrepository;
            this.permissionrepository = permissionrepository;
            this.menuRepository = menuRepository;
            this.classHourFeeSettingRepository = classHourFeeSettingRepository;
            this.logger = logger;
            this.captcha = captcha;
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
                logger.LogError(ex, "成员下拉框获取失败");
                throw;
            }
        }
        /// <summary>分页查询员工信息</summary>
        /// <param name="search">查询条件</param>
        /// <returns>分页结果，包含员工信息</returns>
        [HttpGet]
        //[Authorize]
        public async Task<ApiResult<ApiPaging<List<ShowStaffDTO>>>> GetStaffListAsync([FromQuery] SearchStaffDTO search)
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

                // 获取所有员工的ID集合
                var staffIds = resultList.Select(s => s.Id).ToList();


                // 6. 一次性查询所有相关角色信息（优化性能）
                var staffRoles = await (
                    from sr in await staffrolerepository.GetQueryableAsync()
                    join r in await roleRep.GetQueryableAsync() on sr.RoleId equals r.Id
                    where staffIds.Contains(sr.StaffId)
                    select new { sr.StaffId, r.RoleName }
                ).ToListAsync();

                // 7. 按员工ID分组角色
                var rolesGrouped = staffRoles
                    .GroupBy(x => x.StaffId)
                    .ToDictionary(
                        g => g.Key,
                        g => string.Join(", ", g.Select(x => x.RoleName))
                    );

                foreach (var item in resultList)
                {
                    item.Position = (await positionRep.GetAsync(item.PositionId)).PositionName;
                    item.StaffType = (await typeRep.GetAsync(item.StaffTypeId)).StaffTypeName;
                    // 角色信息
                    item.Role = rolesGrouped.TryGetValue(item.Id, out var roles) ? roles : "无角色";
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
                logger.LogError(ex, "员工信息获取失败");
                throw; // 暂时抛出，可拓展成统一异常处理
            }
        }

        /// <summary>
        /// 批量设置用户所属机构
        /// </summary>
        /// <param name="Ids">要设置的用户ID集合</param>
        /// <param name="organizationIds">要设置的机构ID集合</param>
        /// <returns>操作结果</returns>
        [HttpPost]
        public async Task<ApiResult> StaffOranization([FromQuery] Guid[] Ids, Guid[] organizationIds)
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

                // 使用逗号(,)作为分隔符，将addorUpdStaffDT0.Organization字符串拆分成字符串数组
                string[] organization = addorUpdStaffDTO.Organization.Split(',');

                // 初始化一个空字符串，用于存储最终拼接的结果
                var resultmname = "";

                // 遍历organization数组中的每一个元素
                foreach (var item in organization)
                {
                    // 调用FirstOrderBuildAsync方法查询组织信息，并获取组织名称
                    // 注意：这里有一些特殊符号(&,>,等)可能是占位符或代码片段不完整
                    var organizationname = (await organizationRepository.FirstOrDefaultAsync(x => Convert.ToString(x.Id) == item)).Name;

                    // 将查询到的组织名称拼接到resultmname字符串中，并用分号(;)分隔
                    resultmname += organizationname + ',';
                }

                // 将拼接好的字符串赋值回addorUpdStaffDT0.Organization属性
                // 使用TrimEnd(',')去除末尾可能多余的分号(;)
                // 注意：这里应该使用TrimEnd(';')而不是TrimEnd(',')，因为拼接时使用的是分号
                addorUpdStaffDTO.Organization = resultmname.TrimEnd(',');

                // 将前端传入的 AddorUpdStaffDTO 映射成实体 StaffInfo，用于数据库操作
                var staffinfo = ObjectMapper.Map<AddorUpdStaffDTO, StaffInfo>(addorUpdStaffDTO);

                // 将新员工数据插入数据库
                await basicRepository.InsertAsync(staffinfo);

                // 将插入后的实体对象映射成返回给前端的 ShowStaffDTO
                var showstaffinfo = ObjectMapper.Map<StaffInfo, ShowStaffDTO>(staffinfo);

                //获取机构主键
                var OrganizationId = await organizationRepository.FirstOrDefaultAsync(x => x.Name == staffinfo.Organization);

                ////添加职位表的同时添加薪资表
                //SalarySettingModel salary = new SalarySettingModel() {
                //    StaffId = staffinfo.Id,
                //    OrganizationId= OrganizationId.Id
                //};
                //var a=await salarySettingRepository.InsertAsync(salary); 

                //添加职位表的同时添加薪资表
                SalarySettingModel salary = new SalarySettingModel()
                {
                    StaffId = staffinfo.Id,
                    StaffName = staffinfo.StaffName,
                    OrganizationId = OrganizationId.Id
                };
                var a=await salarySettingRepository.InsertAsync(salary);

                if (a != null)
                {  
                    ClassHourFeeSetting money = new ClassHourFeeSetting()
                    {
                        SalarySettingId = a.Id,
                        ClassHourDuration = 0,
                        ClassHourFee = 0,
                        AssistantFee = 0
                    };
                    await classHourFeeSettingRepository.InsertAsync(money);
                }

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
        public async Task<ApiResult<ShowStaffDTO>> UpdateStaff(Guid staffId, StaffUpdateDTO addorUpdStaffDTO)
        {
            try
            {
                // 根据员工ID查出原始数据
                var staffinfo = await basicRepository.FindAsync(staffId);
                // 使用逗号(,)作为分隔符，将addorUpdStaffDT0.Organization字符串拆分成字符串数组
                string[] organization = addorUpdStaffDTO.Organization.Split(',');

                // 初始化一个空字符串，用于存储最终拼接的结果
                var resultmname = "";

                // 遍历organization数组中的每一个元素
                foreach (var item in organization)
                {
                    // 调用FirstOrderBuildAsync方法查询组织信息，并获取组织名称
                    // 注意：这里有一些特殊符号(&,>,等)可能是占位符或代码片段不完整
                    var organizationname = (await organizationRepository.FirstOrDefaultAsync(x => Convert.ToString(x.Name) == item)).Name;

                    // 将查询到的组织名称拼接到resultmname字符串中，并用分号(;)分隔
                    resultmname += organizationname + ',';
                }

                // 将拼接好的字符串赋值回addorUpdStaffDT0.Organization属性
                // 使用TrimEnd(',')去除末尾可能多余的分号(;)
                // 注意：这里应该使用TrimEnd(';')而不是TrimEnd(',')，因为拼接时使用的是分号
                addorUpdStaffDTO.Organization = resultmname.TrimEnd(',');
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
        public async Task<ApiResult> DeleteStaff([FromQuery] Guid[] Ids)
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
        public virtual async Task<(byte[] FileContent, string FileName)> GetExportStaffList()
        {
            // 获取数据
            var staffinfo = await basicRepository.GetListAsync();

            // 生成文件名
            var fileName = $"员工列表_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            // 调用ExcelExporter
            var fileBytes = ExcelExporter.Export(staffinfo, "员工列表", "员工信息");

            return (fileBytes, fileName);
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

                if (captcha.Validate(loginDTO.CaptchaKey, loginDTO.CaptchaCode) == false)
                {
                    return ApiResult<LoginReturnDTO>.Fail(ResultCode.Fail, "验证码错误");
                }

                var returndto = ObjectMapper.Map<StaffInfo, LoginReturnDTO>(staff);
                returndto.Token = await GenerateJwtTokenAsync(staff);

                // 登录成功，返回用户信息
                return ApiResult<LoginReturnDTO>.Success(ResultCode.Ok, returndto);
            }
            catch (Exception ex)
            {
                // 异常处理（记录日志等）
                logger.LogError(ex, "登录时发生异常");
                throw; // 继续抛出异常
            }
        }

        /// <summary>
        /// 生成JWT令牌
        /// </summary>
        /// <param name="staff">员工实体</param>
        /// <returns>JWT令牌字符串</returns>
        private async Task<string> GenerateJwtTokenAsync(StaffInfo staff)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecurityKey"]);

            // --- 1. 获取用户所有角色 ---
            var staffRoles = await staffrolerepository.GetListAsync(x => x.StaffId == staff.Id);
            var roleIds = staffRoles.Select(sr => sr.RoleId).Distinct().ToList();
            var roles = await roleRep.GetListAsync(r => roleIds.Contains(r.Id));
            var roleNames = roles.Select(r => r.RoleName).ToList();

            // --- 2. 获取用户所有权限Code ---
            var rolePermissions = await rolepermissionrepository.GetListAsync(rp => roleIds.Contains(rp.RoleId));
            var permissionIds = rolePermissions.Select(rp => rp.PermissionId).Distinct().ToList();
            var userPermissions = await permissionrepository.GetListAsync(p => permissionIds.Contains(p.Id));
            var userPermissionCodes = userPermissions.Select(p => p.PermissionCode).ToList();


            //// --- 3. 获取并过滤用户菜单树 ---
            //// 步骤 3.1: 获取原始菜单数据 (假设返回 Educational.Educational.Menu.Menu 类型列表)
            //var rawMenusFromRepo = await menuRepository.GetListAsync();

            //// **步骤 3.2: 使用 AutoMapper 将原始菜单列表映射到共享的 MenuInfo 列表**
            //// 目标类型 Educational.Shared.Models.MenuInfo
            //// allMenus 现在是 List<Educational.Shared.Models.MenuInfo> 类型，与 FilterAndBuildMenuTree 兼容
            //List<Educational.Shared.Models.MenuInfo> allMenus = ObjectMapper.Map<List<Educational.Menu.Menu>,List<Educational.Shared.Models.MenuInfo>>(rawMenusFromRepo);

            //// 步骤 3.3: 使用 Guid.Empty 作为顶级菜单的父级ID，并调用 FilterAndBuildMenuTree
            //var userOwnedMenuTree = FilterAndBuildMenuTree(allMenus, userPermissionCodes, Guid.Empty);


            // --- 4. 组装 Claims ---
            var claims = new List<Claim>
        {
            new Claim("Id", staff.Id.ToString()),
            new Claim("StaffName", staff.StaffName ?? string.Empty),
            new Claim("StaffAccount", staff.StaffAccount ?? string.Empty),
            new Claim("StaffPhone", staff.StaffPhone ?? string.Empty),
            new Claim("StaffGender", staff.StaffGender?.ToString() ?? string.Empty),
            new Claim("StaffTypeId", staff.StaffTypeId.ToString()),
            new Claim("PositionId", staff.PositionId.ToString()),
            new Claim("Organization", staff.Organization ?? string.Empty),
            new Claim("StaffStatus", staff.Status.ToString()),
            new Claim("PhotoUrl", staff.PhotoUrl ?? string.Empty),
            new Claim("Birthday", staff.Birthday?.ToString("yyyy-MM-dd") ?? string.Empty),
            new Claim("Roles", string.Join(",", roleNames)),
            new Claim("Permissions", string.Join(",", userPermissionCodes))
        };

            //// 菜单树信息 - 序列化为JSON字符串
            //if (userOwnedMenuTree != null && userOwnedMenuTree.Any())
            //{
            //    var menuJson = JsonConvert.SerializeObject(userOwnedMenuTree);
            //    claims.Add(new Claim("Menus", menuJson));
            //}


            // --- 5. 创建 SecurityTokenDescriptor 并生成 Token ---
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
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


        public async Task<List<MenuInfo>> GetUserMenusAsync()
        {
            // 获取当前用户的 Id
            // 假设您已经配置了 Abp 的 ICurrentUser 服务来获取当前登录用户的Id
            // 或者从 HttpContext 中获取 Id (如果 Abp 框架没有自动注入)
            // Abp 框架通常通过 CurrentUser.Id 获取
            if (!CurrentUser.IsAuthenticated || CurrentUser.Id == null)
            {
                // 用户未登录或无法获取ID，返回空菜单或抛出异常
                return new List<MenuInfo>();
            }

            var staffId = CurrentUser.Id.Value; // 获取当前登录用户的ID

            // 1. 获取用户所有角色ID
            var staffRoles = await staffrolerepository.GetListAsync(x => x.StaffId == staffId);
            var roleIds = staffRoles.Select(sr => sr.RoleId).Distinct().ToList();

            // 2. 获取用户所有权限Code
            var rolePermissions = await rolepermissionrepository.GetListAsync(rp => roleIds.Contains(rp.RoleId));
            var permissionIds = rolePermissions.Select(rp => rp.PermissionId).Distinct().ToList();
            var userPermissions = await permissionrepository.GetListAsync(p => permissionIds.Contains(p.Id));
            var userPermissionCodes = userPermissions.Select(p => p.PermissionCode).ToList();



            // 3. 获取所有原始菜单数据
            var rawMenusFromRepo = await menuRepository.GetListAsync();

            // 4. 将原始菜单映射到共享的 MenuInfo 列表
            List<MenuInfo> allMenus = ObjectMapper.Map<List<Educational.Menu.Menu>, List<MenuInfo>>(rawMenusFromRepo);
            // 或者手动映射 (如果您之前决定不使用AutoMapper):
            // List<MenuInfo> allMenus = rawMenusFromRepo.Select(src => new MenuInfo { ... }).ToList();

            // 5. 过滤并构建用户有权访问的菜单树
            var userOwnedMenuTree = FilterAndBuildMenuTree(allMenus, userPermissionCodes, Guid.Empty);

            return userOwnedMenuTree;
        }

        /// <summary>
        /// 递归过滤并构建用户有权访问的菜单树
        /// </summary>
        /// <param name="allMenus">所有菜单的扁平列表</param>
        /// <param name="userPermissionCodes">用户拥有的权限Code列表 (从 PermissionInfo.PermissionCode 获取)</param>
        /// <param name="parentId">当前处理的父菜单ID，默认为 Guid.Empty (表示顶级菜单)</param>
        /// <returns>用户有权访问的菜单树</returns>
        private List<MenuInfo> FilterAndBuildMenuTree(List<MenuInfo> allMenus, List<string> userPermissionCodes, Guid parentId)
        {
            // 定义菜单类型的常量或枚举，请根据您的实际定义进行调整
            const int MenuType_Directory = 1; // 假设 1 代表目录
            const int MenuType_MenuItem = 2;  // 假设 2 代表菜单项
                                              // const int MenuType_Button = 3; // 如果有按钮类型，可能不在此处处理

            // 1. 获取当前层级的所有菜单项，并按 MenuSort 排序
            var currentLevelMenus = allMenus
                .Where(m => m.ParentId == parentId)
                .OrderBy(m => m.MenuSort)
                .ToList();

            var visibleMenus = new List<MenuInfo>();

            foreach (var menu in currentLevelMenus)
            {
                // 2. 递归处理子菜单
                // 注意：这里传递的是当前菜单的 Id 作为下一层级的 ParentId
                var children = FilterAndBuildMenuTree(allMenus, userPermissionCodes, menu.Id);

                // 3. 判断当前菜单节点是否应该显示
                bool isCurrentMenuNodeVisible = false;

                // a. 如果菜单没有明确的权限码（或权限码为空字符串/null）
                // 这种菜单通常是纯目录，或者是不需要特定权限的公共菜单项（如仪表盘/首页）
                if (string.IsNullOrWhiteSpace(menu.MenuPermissionCode))
                {
                    // 如果是目录类型菜单，它的可见性取决于其是否有任何可见的子菜单
                    if (menu.MenuType == MenuType_Directory)
                    {
                        isCurrentMenuNodeVisible = children.Any();
                    }
                    // 如果是普通菜单项类型，且没有关联权限码，则默认可见
                    else if (menu.MenuType == MenuType_MenuItem)
                    {
                        isCurrentMenuNodeVisible = true;
                    }
                    // 可以根据需要添加其他 MenuType 的默认可见性逻辑
                    // 例如：如果 MenuType 既不是目录也不是菜单项，且没有权限码，则可能默认不可见
                }
                // b. 如果菜单有明确的权限码 (MenuPermissionCode)
                else
                {
                    // 检查用户拥有的权限Code列表中是否包含该菜单所需的权限码
                    isCurrentMenuNodeVisible = userPermissionCodes.Contains(menu.MenuPermissionCode);
                }

                // 4. 将可见的菜单节点添加到结果列表中
                // 如果当前菜单节点自身可见，则添加到结果中
                if (isCurrentMenuNodeVisible)
                {
                    menu.Children = children; // 将过滤后的子菜单添加到当前菜单对象
                    visibleMenus.Add(menu);
                }
                // 特殊情况：如果当前菜单是目录类型，并且它自身可能没有直接的权限使其可见，
                // 但它的子菜单中存在可见项，那么这个目录也应该被包含进来作为父节点。
                // 这样做是为了保证菜单树的完整性，让前端能正确渲染层级。
                else if (menu.MenuType == MenuType_Directory && children.Any())
                {
                    menu.Children = children; // 包含可见的子菜单
                    visibleMenus.Add(menu);
                }
            }
            return visibleMenus;
        }
    }
}
