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
    }
}
