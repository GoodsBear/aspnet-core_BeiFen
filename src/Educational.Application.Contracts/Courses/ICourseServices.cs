using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.Courses
{
	public interface ICourseServices:IApplicationService
	{
		//添加课程
		Task<ApiResult> AddCourse(CourseDto coursedto);

		//课程分页
		Task<ApiResult<ApiPagiing<List<CourseDto>>>> GetListCourse(SearchCourseDto seach);

	}
}
