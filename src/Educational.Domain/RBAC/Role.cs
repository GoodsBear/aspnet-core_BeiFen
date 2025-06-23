using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.RBAC
{
    public class Role:FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空")] // 必填
        [StringLength(50, MinimumLength = 2, ErrorMessage = "角色名称长度应在2到50个字符之间")] // 长度限制
        public string RoleName { get; set; } = string.Empty; // 默认值为空字符串

        /// <summary>
        /// 角色编码
        /// </summary>
        [Required(ErrorMessage = "角色编码不能为空")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "角色编码长度应在2到50个字符之间")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "角色编码只能包含字母、数字和下划线")] // 通常用于唯一标识符，建议限制字符
        public string RoleEncode { get; set; } = string.Empty; // 默认值为空字符串

        /// <summary>
        /// 角色描述
        /// </summary>
        [StringLength(255, ErrorMessage = "角色描述长度不能超过255个字符")] // 描述通常允许更长
        public string RoleDesc { get; set; } = string.Empty; // 默认值为空字符串

        /// <summary>
        /// 角色状态
        /// </summary>
        [Required(ErrorMessage = "角色状态不能为空")]
        [RegularExpression(@"^[01]$", ErrorMessage = "角色状态只能是 '0' (禁用) 或 '1' (启用)")] // 严格限制为 "0" 或 "1"
        public int RoleStatus { get; set; } // 1:启用, 0:禁用
    }
}
