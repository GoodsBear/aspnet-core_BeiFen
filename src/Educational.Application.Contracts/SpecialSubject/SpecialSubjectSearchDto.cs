using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.SpecialSubject
{
    public class SpecialSubjectSearchDto:Seach
    {
        /// <summary>
        /// 专题名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 专题类别
        /// </summary>
        public Guid? CategoryId { get; set; }
        /// <summary>
        /// 授课老师
        /// </summary>
        public string? Teacher { get; set; }

    }
}
