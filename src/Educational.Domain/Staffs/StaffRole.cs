using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Staffs
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class StaffRole : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public Guid StaffId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "角色Id不能为空")]
        public Guid RoleId { get; set; }
    }
}
