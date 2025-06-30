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
		Task<ApiResult> AddCourse(CreateCourseDto coursedto);

		//课程分页
		Task<ApiResult<ApiPaging<List<CourseDto>>>> GetListCourse(SearchCourseDto seach);

		//批量修改课程状态
		Task<ApiResult> UpdateCourseStatus(List<Guid> guids, bool status,int type);
		// 获取课程下拉框
		Task<ApiResult<List<CourseSelectDto>>> GetCourseAsync();


    }
}
