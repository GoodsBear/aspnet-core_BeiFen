using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.SpecialSubject
{
    public class CategoryModelDto 
    { 
        public Guid Id { get; set; }
        /// <summary>
        /// 专题类别名称
        /// </summary>
        public string CategoryName { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
