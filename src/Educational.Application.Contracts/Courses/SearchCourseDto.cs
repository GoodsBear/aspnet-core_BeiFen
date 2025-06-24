using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Courses
{
	public class SearchCourseDto:Seach
	{
		/// <summary>
		/// 课程名称
		/// </summary>
		public string? CourseName { get; set; }
		/// <summary>
		/// 适用学校
		/// </summary>
		public string? CampusName { get; set; }
		/// <summary>
		/// 科目
		/// </summary>
		public string? SubjectName { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public bool? Status { get; set; }
		//适用年级
        public Guid GradeId { get; set; }
	}
}
