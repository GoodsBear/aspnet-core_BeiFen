using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Menu
{
    /// <summary>
    /// 角色菜单关系表
    /// </summary>
    public class MenuPermissionsRelation:FullAuditedAggregateRoot<Guid>
    {
        public Guid Permission { get; set; }

        public Guid MenuId { get; set; }
    }
}
