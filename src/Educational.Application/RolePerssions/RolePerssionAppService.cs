using Educational.RBAC;
using Educational.RBAC.PermissionsManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IBasicRepository<Permissions, Guid> permissionrepository;
        private readonly ILogger<RolePerssionAppService> logger;

        public RolePerssionAppService(IRepository<RolePermission, Guid> rolePermissionRepository, IRepository<Role, Guid> roleRepository, IBasicRepository<Permissions, Guid> permissionrepository, ILogger<RolePerssionAppService> logger)
        {
            this.rolePermissionRepository = rolePermissionRepository;
            this.roleRepository = roleRepository;
            this.permissionrepository = permissionrepository;
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
        /// <summary>
        /// 通过角色Id获取对应权限
        /// </summary>
        /// <param name="StaffId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<List<PermissionsDto>>> GetRoleIdToFindPermission(Guid RoleId)
        {
            try
            {
                // 1. 获取角色下所有权限分配
                var rolePermissions = await rolePermissionRepository.GetListAsync(x => x.RoleId == RoleId);
                if (rolePermissions == null || !rolePermissions.Any())
                {
                    return ApiResult<List<PermissionsDto>>.Fail(ResultCode.Fail, "未找到对应权限");
                }

                // 2. 获取所有权限ID
                var permissionIds = rolePermissions.Select(p => p.PermissionId).ToList();

                // 3. 循环查找每个权限的详细信息
                var permissionDtos = new List<PermissionsDto>();
                foreach (var permissionId in permissionIds)
                {
                    var permission = await permissionrepository.FindAsync(permissionId);
                    if (permission != null)
                    {
                        var dto = new PermissionsDto
                        {
                            Id = permission.Id,
                            PermissionName = permission.PermissionName,
                            PermissionsDesc = permission.PermissionsDesc
                        };
                        permissionDtos.Add(dto);
                    }
                }

                return ApiResult<List<PermissionsDto>>.Success(ResultCode.Ok, permissionDtos);
            }
            catch (Exception ex)
            {
                logger.LogError("权限获取出错" + ex.Message);
                throw;
            }
        }
    }
}
