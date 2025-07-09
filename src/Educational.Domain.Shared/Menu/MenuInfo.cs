using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Shared.Models
{
    public class MenuInfo
    {
        public Guid Id { get; set; }
        public string MenuName { get; set; }
        public string MenuPath { get; set; }
        public string MenuIcon { get; set; }
        public Guid ParentId { get; set; }
        public string MenuPermissionCode { get; set; } // 关联的权限标识符
        public int MenuType { get; set; } // 1:目录 2:菜单 3:按钮
        public string MenuSort { get; set; } //菜单排序

        // 新增属性，用于构建树形结构
        public List<MenuInfo> Children { get; set; } = new List<MenuInfo>();
    }
}
