using Educational.Courses.ReletedCoursedtos;
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
		//修改课程信息
        Task<ApiResult<Course>> UpdateCourse(CreateUpdateCourseDto coursedto);

        Task<ApiResult<List<CourseDto>>> FTCouser();

		/// <summary>
		/// 添加关联课程
		/// </summary>
		/// <param name="guid">课程id</param>
		/// <param name="ids">要关联的课程id数组</param>
		/// <returns></returns>
		Task<ApiResult> AddReletedCourse(ReletedCourseDto dto);

		/// <summary>
		/// 获取关联课程
		/// </summary>
		/// <param name="guid">课程id</param>
		/// <returns></returns>
		Task<ApiResult<ApiPaging<List<CourseDto>>>> GetReletedCourse(SearchReletedCourseDto search);

		/// <summary>
		/// 移除关联课程
		/// </summary>
		/// <param name="guid">关联课程id</param>
		/// <param name="id">主课程id</param>
		/// <returns></returns>
        Task<ApiResult> RemoveReletedCourse(Guid guid, Guid id);

		/// <summary>
		/// 课程下拉
		/// </summary>
		/// <param name="guid">关联课程id</param>
		/// <param name="id">主课程id</param>
		/// <returns></returns>
        Task<ApiResult<List<CourseDto>>> FTCouser();
    }
}
