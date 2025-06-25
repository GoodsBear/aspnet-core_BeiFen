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
        private readonly ILogger<StafRoleAppService> logger;

        public StafRoleAppService(IRepository<StaffRole, Guid> staffRoleRepository, ILogger<StafRoleAppService> logger, IRepository<StaffInfo, Guid> staffRepository)
        {
            this.staffRoleRepository = staffRoleRepository;
            this.logger = logger;
            this.staffRepository = staffRepository;
        }

        /// <summary>
        /// 添加修改用户角色分配
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

    }
}
