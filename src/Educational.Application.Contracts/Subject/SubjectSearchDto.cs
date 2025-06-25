using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Subject
{
    public class SubjectSearchDto:Seach
    {
        /// <summary>
        /// 科目名称--模糊
        /// </summary>
        public string? SubjectName { get; set; }
    }
}
