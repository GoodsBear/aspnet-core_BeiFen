using Educational.RBAC;
using Educational.Staffs;
using Educational.StafRoles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.RolePerssions
{
    [ApiExplorerSettings(GroupName = "角色分配权限")]
    public class RolePerssionAppService : ApplicationService, IRolePerssionAppService
    {
        private readonly IRepository<RolePermission, Guid> rolePermissionRepository;
        private readonly IRepository<Role, Guid> roleRepository;
        private readonly ILogger<RolePerssionAppService> logger;

        public RolePerssionAppService(IRepository<RolePermission, Guid> rolePermissionRepository, IRepository<Role, Guid> roleRepository, ILogger<RolePerssionAppService> logger)
        {
            this.rolePermissionRepository = rolePermissionRepository;
            this.roleRepository = roleRepository;
            this.logger = logger;
        }
        /// <summary>
        /// 添加修改角色权限分配
        /// </summary>
        /// <param name="input"> 角色分配权限dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult> AddRolePermission(RolePermissionDto input)
        {
            try
            {
                // 校验角色是否存在
                var role = await roleRepository.FindAsync(input.RoleId);
                if (role == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "角色不存在");
                }
                // 删除原有权限分配
                var oldPerssion = await rolePermissionRepository.GetListAsync(x => x.RoleId == input.RoleId);
                if (oldPerssion.Any())
                {
                    await rolePermissionRepository.DeleteManyAsync(oldPerssion);
                }
                var createRoles = input.PermissionIds.Select(perssion => new RolePermission
                {
                    RoleId = input.RoleId,
                    PermissionId = perssion
                }).ToList();
                await rolePermissionRepository.InsertManyAsync(createRoles);
                return ApiResult.Success(ResultCode.Ok);

            }
            catch (Exception ex)
            {
                logger.LogError($"添加角色权限分配失败: {ex.Message}");
                throw;
            }
        }
    }
}
