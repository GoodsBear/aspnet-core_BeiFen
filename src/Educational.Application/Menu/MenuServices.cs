using Educational.RBAC.RoleManager;
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

namespace Educational.Menu
{
    [ApiExplorerSettings(GroupName = "菜单")]
    public class MenuServices : ApplicationService, IMenuServices
    {
        private readonly IRepository<Menu, Guid> repository;
        private readonly ILogger<MenuServices> logger;

        public MenuServices(IRepository<Menu, Guid> repository, ILogger<MenuServices> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        [HttpPost]
        public async Task<ApiResult<MenuDto>> AddMenu(CreateUpdateMenuDto createUpdateMenuDto)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // 检查菜单名称是否已存在
                    var list = await repository.GetListAsync(d => d.MenuName == createUpdateMenuDto.MenuName);
                    if (list.Count != 0)
                    {
                        return ApiResult<MenuDto>.Fail(ResultCode.Fail, "菜单名称已存在，请更换菜单名称！");
                    }

                    var data = ObjectMapper.Map<CreateUpdateMenuDto, Menu>(createUpdateMenuDto);
                    var res = await repository.InsertAsync(data);

                    if (res != null)
                    {
                        var menuDto = ObjectMapper.Map<Menu, MenuDto>(res);
                        tran.Complete();
                        return ApiResult<MenuDto>.Success(ResultCode.Ok, menuDto);
                    }
                    else
                    {
                        return ApiResult<MenuDto>.Fail(ResultCode.Fail, "菜单添加失败");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("菜单添加出错" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        [HttpDelete]
        public async Task<ApiResult> DelMenu(Guid guid)
        {
            try
            {
                await repository.DeleteAsync(guid);
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("菜单删除失败" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        [HttpGet]
        public async Task<ApiResult<List<MenuDto>>> GetMenuList()
        {
            try
            {
                var list = await repository.GetListAsync();
                if (list == null || list.Count == 0)
                {
                    return ApiResult<List<MenuDto>>.Fail(ResultCode.Fail, "没有菜单数据");
                }
                var data = ObjectMapper.Map<List<Menu>, List<MenuDto>>(list);
                return ApiResult<List<MenuDto>>.Success(ResultCode.Ok, data);
            }
            catch (Exception ex)
            {
                logger.LogError("获取菜单列表出错: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        [HttpPut]
        public async Task<ApiResult<MenuDto>> UpdateMenu(CreateUpdateMenuDto createUpdateMenuDto, Guid guid)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var entity = await repository.GetAsync(guid);
                    if (entity == null)
                    {
                        return ApiResult<MenuDto>.Fail(ResultCode.Fail, "菜单不存在，请检查！");
                    }

                    var data = ObjectMapper.Map(createUpdateMenuDto, entity);
                    var res = await repository.UpdateAsync(data);

                    if (res != null)
                    {
                        var menuDto = ObjectMapper.Map<Menu, MenuDto>(res);
                        tran.Complete();
                        return ApiResult<MenuDto>.Success(ResultCode.Ok, menuDto);
                    }
                    else
                    {
                        return ApiResult<MenuDto>.Fail(ResultCode.Fail, "菜单修改失败");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("菜单修改出错" + ex.Message);
                throw;
            }
        }
    }
}
