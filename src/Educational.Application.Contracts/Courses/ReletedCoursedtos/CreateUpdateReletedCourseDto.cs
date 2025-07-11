using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Courses.ReletedCoursedtos
{
	/// <summary>
	/// 创建或修改关联课程的dto参数
	/// </summary>
	public class CreateUpdateReletedCourseDto
	{
		public Guid Id { get; set; }	
		public List<Guid> ids { get; set; }
	}
}
