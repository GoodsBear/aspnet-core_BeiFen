using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.StafRoles
{
    public interface IStafRoleAppService : IApplicationService
    {
        /// <summary>
        /// 添加用户角色分配
        /// </summary>
        Task<ApiResult> AddStaffRole(StaffRoleDto input);
        /// <summary>
        /// 添加角色用户分配
        /// </summary>
        Task<ApiResult> AddRoleStaff(RoleStaffDto input);
        /// <summary>
        /// 移除角色用户
        /// </summary>
        Task<ApiResult> RemoveStaffFromRole(Guid roleId, List<Guid> staffIds);
    }
}
