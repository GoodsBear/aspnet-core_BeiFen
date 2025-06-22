using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.RBAC
{
    public class Permissions:FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        [Required(ErrorMessage = "权限名称不能为空")] // 必填
        [StringLength(50, MinimumLength = 2, ErrorMessage = "权限名称长度应在2到50个字符之间")] // 限制长度，例如2到50个字符
        public string PermissionName { get; set; } = string.Empty;

        /// <summary>
        /// 权限描述
        /// </summary>
        [StringLength(255, ErrorMessage = "权限描述长度不能超过255个字符")] // 描述字段通常允许较长的文本
        public string PermissionsDesc { get; set; } = string.Empty;
    }
}
