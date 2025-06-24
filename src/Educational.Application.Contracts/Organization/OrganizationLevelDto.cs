using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Organization
{
    public class OrganizationLevelDto : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 机构级别
        /// </summary> 
        public string Name { get; set; }
    }
}
