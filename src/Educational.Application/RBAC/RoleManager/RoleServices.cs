using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Yitter.IdGenerator;

namespace Educational.RBAC.RoleManager
{
    [ApiExplorerSettings(GroupName = "角色")]
    public class RoleServices : ApplicationService, IRoleServices
    {
        private readonly IRepository<Role, Guid> repository;
        private readonly ILogger<RoleServices> logger;

        public RoleServices(IRepository<Role, Guid> repository, ILogger<RoleServices> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="createUpdateRole"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<RoleDto>> AddRole(CreateUpdateRoleDto createUpdateRole)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //查询角色名称是否存在
                    var list = await repository.GetListAsync(d => d.RoleName == createUpdateRole.RoleName);
                    if (list.Count != 0)
                    {
                        return ApiResult<RoleDto>.Fail(ResultCode.Fail, "角色名称已存在，请更换角色名称！");
                    }

                    var data = ObjectMapper.Map<CreateUpdateRoleDto, Role>(createUpdateRole);

                    //给角色编码雪花Id
                    data.RoleEncode = YitIdHelper.NextId().ToString();

                    var res = await repository.InsertAsync(data);

                    if (res != null)
                    {

                        var roleDto = ObjectMapper.Map<Role, RoleDto>(res);
                        //提交事务
                        tran.Complete();

                        return ApiResult<RoleDto>.Success(ResultCode.Ok, roleDto);
                    }
                    else
                    {
                        return ApiResult<RoleDto>.Fail(ResultCode.Fail, "角色添加失败");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("角色添加出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DelRole(Guid guid)
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
        /// 角色列表
        /// </summary>
        /// <param name="searchRoleDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<ApiPaging<List<RoleDto>>>> PageRole([FromQuery]SearchRoleDto searchRoleDto)
        {
            try
            {
                var list = await repository.GetQueryableAsync();

                list = list.WhereIf(!string.IsNullOrEmpty(searchRoleDto.RoleName), d => d.RoleName.Equals(searchRoleDto.RoleName));

                var page = list.PageResult(searchRoleDto.PageIndex, searchRoleDto.PageSize);

                var roleDtos = ObjectMapper.Map<List<Role>, List<RoleDto>>(page.Queryable.ToList());

                ApiPaging<List<RoleDto>> paging = new ApiPaging<List<RoleDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling((page.RowCount*1.0)/page.PageSize),
                    Data = roleDtos
                };
                return ApiResult<ApiPaging<List<RoleDto>>>.Success(ResultCode.Ok, paging);
            }
            catch (Exception ex)
            {
                logger.LogError("角色列表出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="createUpdateRoleDto"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<RoleDto>> UpdateRole(CreateUpdateRoleDto createUpdateRoleDto,Guid guid)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var list = await repository.GetAsync(guid);
                    if (list == null)
                    {
                        return ApiResult<RoleDto>.Fail(ResultCode.Fail, "角色不存在，请检查！");
                    }

                    var data = ObjectMapper.Map(createUpdateRoleDto, list);
                    data.RoleEncode = YitIdHelper.NextId().ToString();
                    var res = await repository.UpdateAsync(data);

                    if (res != null)
                    {
                        var roleDto = ObjectMapper.Map<Role, RoleDto>(res);
                        tran.Complete();
                        return ApiResult<RoleDto>.Success(ResultCode.Ok, roleDto);
                    }
                    else
                    {
                        return ApiResult<RoleDto>.Fail(ResultCode.Fail, "角色修改失败");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("角色修改出错"+ex.Message);
                throw;
            }
        }
    }
}
