using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Organization
{
    public class OrganizationLevel : FullAuditedAggregateRoot<Guid>
    {  
        /// <summary>
        /// 机构级别
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "机构名长度不能超过100字符")]
        [Display(Name = "机构级别")]
        public string Name { get; set; } 
    }
}

