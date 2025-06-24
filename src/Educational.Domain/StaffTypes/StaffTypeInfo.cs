using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.StaffTypes
{
    public class StaffTypeInfo:FullAuditedAggregateRoot<Guid>
    {
        [Required]
        [MaxLength(100)]
        [Comment("人员类型")]
        public string StaffTypeName { get; set; }=string.Empty;
    }
}
