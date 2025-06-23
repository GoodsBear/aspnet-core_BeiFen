using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.RBAC.PermissionsManager
{
    public interface IPermissionsServices : IApplicationService
    {
        /// <summary>
        /// 添加权限
        /// </summary>
        Task<ApiResult<PermissionsDto>> AddPermissions(CreateUpdatePermissionsDto createUpdatePermissions);

        /// <summary>
        /// 权限分页查询
        /// </summary>
        Task<ApiResult<ApiPaging<List<PermissionsDto>>>> PagePermissions(SearchPermissionsDto searchPermissionsDto);

        /// <summary>
        /// 删除权限
        /// </summary>
        Task<ApiResult> DelPermissions(Guid guid);

        /// <summary>
        /// 更新权限
        /// </summary>
        Task<ApiResult<PermissionsDto>> UpdatePermissions(CreateUpdatePermissionsDto createUpdatePermissionsDto, Guid guid);
        /// <summary>
        /// 获取权限树形结构
        /// </summary>
        /// <returns>返回树形结构的权限列表</returns>
        Task<ApiResult<List<PermissionsTreeDto>>> GetPermissionsTree();

    }
}
