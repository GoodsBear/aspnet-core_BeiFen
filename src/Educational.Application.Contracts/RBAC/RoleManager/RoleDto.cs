using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.RBAC.RoleManager
{
    public class RoleDto :FullAuditedAggregateRoot<Guid>
    {
        public string RoleName { get; set; }

        public string RoleEncode { get; set; }

        public string RoleDesc { get; set; }

        public int RoleStatus { get; set; }
    }
}
