using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Menu
{
    public class MenuDto : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        // ParentId 可以是 Guid.Empty，表示顶级菜单，所以不强制要求
        public Guid ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; }

        /// <summary>
        /// 菜单组件对应的前端路径
        /// </summary>
        public string MenuComponent { get; set; }

        /// <summary>
        /// 菜单图标、图标Url
        /// </summary>
        public string MenuIcon { get; set; }

        /// <summary>
        /// 菜单排序顺序
        /// </summary>
        // MenuSort 如果是字符串，可以考虑使用正则表达式验证格式，或者转换为 int 类型
        // 如果希望它是数字字符串，可以添加正则表达式验证
        // [RegularExpression(@"^\d+$", ErrorMessage = "菜单排序顺序必须是数字。")]
        public string MenuSort { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public int MenuType { get; set; } // 1:目录 2:菜单 3:按钮

        /// <summary>
        /// 菜单权限标识
        /// </summary>
        public string MenuPermission { get; set; }

        /// <summary>
        /// 菜单状态
        /// </summary>
        public int MenuStatus { get; set; } // 1:启用 2:禁用

        /// <summary>
        /// 是否隐藏菜单
        /// </summary>
        public bool MenuHidden { get; set; }

        /// <summary>
        /// 菜单描述
        /// </summary>
        public string MenuRemark { get; set; }
    }
}
