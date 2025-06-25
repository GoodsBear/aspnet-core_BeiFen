using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.RBAC
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class RolePermission : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 权限id
        /// </summary>
        public Guid PermissionId { get; set; }
    }
}
