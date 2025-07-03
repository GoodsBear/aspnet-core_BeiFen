using Educational.RBAC.PermissionsManager;
using Educational.StafRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.RolePerssions
{
    public interface IRolePerssionAppService : IApplicationService
    {
        /// <summary>
        /// 添加角色分配权限
        /// </summary>
        Task<ApiResult> AddRolePermission(RolePermissionDto input);
        /// <summary>
        /// 通过角色Id获取对应权限
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        Task<ApiResult<List<PermissionsDto>>> GetRoleIdToFindPermission(Guid RoleId);
    }
}
