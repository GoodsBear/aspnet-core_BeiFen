using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.RBAC
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class StafRole : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid StaffId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set;}
    }
}
