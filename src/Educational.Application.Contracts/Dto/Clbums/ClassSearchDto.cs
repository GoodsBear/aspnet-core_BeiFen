using Educational.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.Clbums
{
    public class ClassSearchDto:Seach
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        public string? ClassName { get; set; }
        /// <summary>
        /// 分校
        /// </summary>
        public Guid? CampusId { get; set; }
        /// <summary>
        /// 默认课程
        /// </summary>
        public Guid? DefaultCourseId { get; set; }
        /// <summary>
        /// 默认教室
        /// </summary>
        public Guid? DefaultClassroomId { get; set; }
        /// <summary>
        /// 班主任
        /// </summary>
        public Guid? ClassTeacherId { get; set; }
        /// <summary>
        /// 年级
        /// </summary>
        public Guid? GradeId { get; set; }
        /// <summary>
        /// 班级状态
        /// </summary>
        public int? ClassStatus { get; set; }
    }
}
