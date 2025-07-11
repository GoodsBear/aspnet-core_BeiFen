using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Courses.ReletedCoursedtos
{
	/// <summary>
	/// 课程关联Dto用于添加课程关联
	/// </summary>
	public class ReletedCourseDto
	{
        public Guid CourseId { get; set; }
        public List<Guid> Guids { get; set; }
	}
}
