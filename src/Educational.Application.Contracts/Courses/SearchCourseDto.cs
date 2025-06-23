using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Courses
{
	public class SearchCourseDto:Seach
	{
		public string? CourseName { get; set; }

		public string? CampusName { get; set; }

		public string? SubjectName { get; set; }

		public bool? Status { get; set; }
	}
}
