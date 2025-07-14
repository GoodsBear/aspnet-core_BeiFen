using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StudentsAndParents.EnrollmentRecords
{
    public class EnrollmentSeachDto:Seach
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? EndTime { get; set; }
        /// <summary>
        /// 课程
        /// </summary>
        public Guid? CourseId { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public Guid? Consultant { get; set; }
    }
}
