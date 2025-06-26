using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.SpecialSubject
{
    /// <summary>
    /// 专题级别
    /// </summary>
    public class CategoryModel : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 专题类别名称
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "专题级别名称长度不能超过100字符")]
        [Display(Name = "专题级别名称")]
        public string CategoryName { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
