using Educational.RBAC.RoleManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Menu
{
    public interface IMenuServices
    {
        /// <summary>
        /// 菜单添加
        /// </summary>
        Task<ApiResult<MenuDto>> AddMenu(CreateUpdateMenuDto createUpdateMenuDto);
        /// <summary>
        /// 菜单
        /// </summary>
        Task<ApiResult<List<MenuDto>>> GetMenuList();
        /// <summary>
        /// 删除菜单
        /// </summary>
        Task<ApiResult> DelMenu(Guid guid);
        /// <summary>
        /// 更新菜单
        /// </summary>
        Task<ApiResult<MenuDto>> UpdateMenu(CreateUpdateMenuDto createUpdateMenuDto, Guid guid);
        /// <summary>
        /// 菜单树形结构列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<MenuTreeDto>>> MenuTreeList();
    }
}
