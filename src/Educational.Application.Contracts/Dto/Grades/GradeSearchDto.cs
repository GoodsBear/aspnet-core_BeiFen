using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.Grades
{
    public class GradeSearchDto:Seach
    {
        /// <summary>
		/// 年级名称
		/// </summary>
		public string? GradeName { get; set; }
        /// <summary>
        /// 入学年份
        /// </summary>
        public string? EnrollYear { get; set; }
    }
}
