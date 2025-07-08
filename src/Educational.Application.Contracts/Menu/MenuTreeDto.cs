using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Menu
{
    public class MenuTreeDto
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; } // 如果前端需要父级ID来构建树，可以保留
        public string MenuName { get; set; } //菜单名称
        public string MenuPath { get; set; } //菜单路径
        public string MenuComponent { get; set; } // 前端组件路径
        public string MenuIcon { get; set; } //图标
        public string MenuSort { get; set; } // 如果前端需要排序，可以保留
        public string MenuPermissionCode { get; set; }//权限标识符
        public int MenuType { get; set; } // 1:目录 2:菜单 3:按钮

        // 注意：MenuPermission, MenuStatus, MenuHidden, MenuRemark 这些字段通常不需要返回给前端，
        // 因为前端只关心如何渲染菜单以及导航。权限判断是在后端完成的。

        public List<MenuTreeDto> Children { get; set; } = new List<MenuTreeDto>();
    }
}
