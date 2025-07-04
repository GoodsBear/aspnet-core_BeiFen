using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.RBAC.RoleManager
{
    public interface IRoleServices :IApplicationService
    {
        /// <summary>
        /// 角色添加
        /// </summary>
        Task<ApiResult<RoleDto>> AddRole(CreateUpdateRoleDto createUpdateRole);
        /// <summary>
        /// 角色列表查询
        /// </summary>
        Task<ApiResult<ApiPaging<List<RoleDto>>>> PageRole(SearchRoleDto searchRoleDto);
        /// <summary>
        /// 删除角色
        /// </summary>
        Task<ApiResult> DelRole(Guid guid);
        /// <summary>
        /// 更新角色
        /// </summary>
        Task<ApiResult<RoleDto>> UpdateRole(CreateUpdateRoleDto createUpdateRoleDto,Guid guid);
        /// <summary>
        /// 角色下拉框
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<SelRoleDto>>> SelRole();
    }
}
