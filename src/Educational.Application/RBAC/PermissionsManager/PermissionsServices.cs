using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Yitter.IdGenerator;

namespace Educational.RBAC.PermissionsManager
{
    [ApiExplorerSettings(GroupName = "权限")]
    public class PermissionsServices : ApplicationService, IPermissionsServices
    {
        private readonly IRepository<Permissions, Guid> repository;
        private readonly ILogger<PermissionsServices> logger;

        public PermissionsServices(IRepository<Permissions, Guid> repository, ILogger<PermissionsServices> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="createUpdatePermissions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<PermissionsDto>> AddPermissions(CreateUpdatePermissionsDto createUpdatePermissions)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //查询权限名称是否存在
                    var list = await repository.GetListAsync(d => d.PermissionName == createUpdatePermissions.PermissionName);
                    if (list.Count != 0)
                    {
                        return ApiResult<PermissionsDto>.Fail(ResultCode.Fail, "权限名称已存在，请更换权限名称！");
                    }

                    var data = ObjectMapper.Map<CreateUpdatePermissionsDto, Permissions>(createUpdatePermissions);


                    var res = await repository.InsertAsync(data);

                    if (res != null)
                    {
                        var permissionsDto = ObjectMapper.Map<Permissions, PermissionsDto>(res);
                        //提交事务
                        tran.Complete();

                        return ApiResult<PermissionsDto>.Success(ResultCode.Ok, permissionsDto);
                    }
                    else
                    {
                        return ApiResult<PermissionsDto>.Fail(ResultCode.Fail, "权限添加失败");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("权限添加出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DelPermissions(Guid guid)
        {
            try
            {
                await repository.DeleteAsync(guid);
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("删除失败" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获取权限树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<PermissionsTreeDto>>> GetPermissionsTree()
        {
            try
            {
                var allPermissions = await repository.GetListAsync();
                var rootPermissions = allPermissions
                    .Where(x => x.ParentId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    .ToList();

                var treeList = rootPermissions.Select(root => new PermissionsTreeDto
                {
                    value = root.Id,
                    label = root.PermissionName,
                    children = allPermissions
                        .Where(x => x.ParentId == root.Id)
                        .Select(child => new PermissionsTreeDto
                        {
                            value = child.Id,
                            label = child.PermissionName,
                            children = null // 二级菜单不递归
                        })
                        .ToList()
                }).ToList();

                return ApiResult<List<PermissionsTreeDto>>.Success(ResultCode.Ok, treeList);
            }
            catch (Exception ex)
            {
                logger.LogError("获取权限树形结构出错: " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 分页查询权限
        /// </summary>
        /// <param name="searchPermissionsDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<ApiPaging<List<PermissionsDto>>>> PagePermissions([FromQuery]SearchPermissionsDto searchPermissionsDto)
        {
            try
            {
                var list = await repository.GetQueryableAsync();

                list = list.WhereIf(!string.IsNullOrEmpty(searchPermissionsDto.PermissionName), 
                                  d => d.PermissionName.Equals(searchPermissionsDto.PermissionName));

                var page = list.PageResult(searchPermissionsDto.PageIndex, searchPermissionsDto.PageSize);

                var permissionsDtos = ObjectMapper.Map<List<Permissions>, List<PermissionsDto>>(page.Queryable.ToList());

                ApiPaging<List<PermissionsDto>> paging = new ApiPaging<List<PermissionsDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling((page.RowCount * 1.0) / page.PageSize),
                    Data = permissionsDtos
                };
                return ApiResult<ApiPaging<List<PermissionsDto>>>.Success(ResultCode.Ok, paging);
            }
            catch (Exception ex)
            {
                logger.LogError("权限列表出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="createUpdatePermissionsDto"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<PermissionsDto>> UpdatePermissions(CreateUpdatePermissionsDto createUpdatePermissionsDto, Guid guid)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var list = await repository.GetAsync(guid);
                    if (list == null)
                    {
                        return ApiResult<PermissionsDto>.Fail(ResultCode.Fail, "权限不存在，请检查！");
                    }

                    var data = ObjectMapper.Map(createUpdatePermissionsDto, list);
                    var res = await repository.UpdateAsync(data);

                    if (res != null)
                    {
                        var permissionsDto = ObjectMapper.Map<Permissions, PermissionsDto>(res);
                        tran.Complete();
                        return ApiResult<PermissionsDto>.Success(ResultCode.Ok, permissionsDto);
                    }
                    else
                    {
                        return ApiResult<PermissionsDto>.Fail(ResultCode.Fail, "权限修改失败");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("权限修改出错" + ex.Message);
                throw;
            }
        }
    }
}
