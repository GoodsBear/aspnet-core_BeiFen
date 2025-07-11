using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Courses
{
    /// <summary>
    /// 课程下拉返回dto
    /// </summary>
    public class CourseSelectDto
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
    }
}
