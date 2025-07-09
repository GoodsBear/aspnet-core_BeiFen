using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Menu
{
    /// <summary>
    /// 菜单表
    /// </summary>
    public class Menu : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        // ParentId 可以是 Guid.Empty，表示顶级菜单，所以不强制要求
        public Guid ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required(ErrorMessage = "菜单名称不能为空。")] // 必填项
        [StringLength(50, MinimumLength = 2, ErrorMessage = "菜单名称长度应在2到50个字符之间。")] // 字符串长度限制
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        [Required(ErrorMessage = "菜单路径不能为空。")]
        [StringLength(200, ErrorMessage = "菜单路径长度不能超过200个字符。")]
        public string MenuPath { get; set; }

        /// <summary>
        /// 菜单组件对应的前端路径
        /// </summary>
        [StringLength(200, ErrorMessage = "菜单组件路径长度不能超过200个字符。")]
        public string MenuComponent { get; set; }

        /// <summary>
        /// 菜单图标、图标Url
        /// </summary>
        [StringLength(200, ErrorMessage = "菜单图标长度不能超过200个字符。")]
        public string MenuIcon { get; set; }

        /// <summary>
        /// 菜单排序顺序
        /// </summary>
        // MenuSort 如果是字符串，可以考虑使用正则表达式验证格式，或者转换为 int 类型
        [StringLength(10, ErrorMessage = "菜单排序顺序长度不能超过10个字符。")]
        // 如果希望它是数字字符串，可以添加正则表达式验证
        // [RegularExpression(@"^\d+$", ErrorMessage = "菜单排序顺序必须是数字。")]
        public string MenuSort { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        [Required(ErrorMessage = "菜单类型不能为空。")]
        [Range(1, 3, ErrorMessage = "菜单类型必须是1（目录）、2（菜单）或3（按钮）。")] // 范围验证
        public int MenuType { get; set; } // 1:目录 2:菜单 3:按钮

        /// <summary>
        /// 菜单权限标识
        /// </summary>
        [StringLength(100, ErrorMessage = "菜单权限标识长度不能超过100个字符。")]
        public string MenuPermissionCode { get; set; }

        /// <summary>
        /// 菜单状态
        /// </summary>
        [Required(ErrorMessage = "菜单状态不能为空。")]
        [Range(1, 2, ErrorMessage = "菜单状态必须是1（启用）或2（禁用）。")] // 范围验证
        public int MenuStatus { get; set; } // 1:启用 2:禁用

        /// <summary>
        /// 是否隐藏菜单
        /// </summary>
        [Required(ErrorMessage = "是否隐藏菜单不能为空。")] // 布尔值通常也建议必填
        public bool MenuHidden { get; set; }

        /// <summary>
        /// 菜单描述
        /// </summary>
        [StringLength(500, ErrorMessage = "菜单描述长度不能超过500个字符。")]
        public string MenuRemark { get; set; }
    }
}
