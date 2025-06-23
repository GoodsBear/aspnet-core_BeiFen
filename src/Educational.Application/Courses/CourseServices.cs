using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Courses
{
	public class CourseServices : ApplicationService, ICourseServices
	{
		IRepository<Course, Guid> _courseRepository;

		public CourseServices(IRepository<Course, Guid> courseRepository)
		{
			_courseRepository = courseRepository;
		}

		public async Task<ApiResult> AddCourse(CourseDto coursedto)
		{
			try
			{
				var course = ObjectMapper.Map<CourseDto, Course>(coursedto);
				var cour=await _courseRepository.InsertAsync(course);
				var res=cour.Equals(course);
				return ApiResult.Success(ResultCode.Ok);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<ApiResult<ApiPagiing<List<CourseDto>>>> GetListCourse([FromQuery]SearchCourseDto seach)
		{
			var course=await _courseRepository.GetQueryableAsync();
			var coursepage= course.Page(seach.PageIndex,seach.PageSize);
			var courselist=ObjectMapper.Map<List<Course>, List<CourseDto>>(coursepage.ToList());
			ApiPagiing<List<CourseDto>> paging=new ApiPagiing<List<CourseDto>>
			{
				TotleCount= course.Count(),
				Data= courselist,
                TotlePage= (int)Math.Ceiling(course.Count()* 1.0/seach.PageSize)
			};
			return ApiResult<ApiPagiing<List<CourseDto>>>.Success(ResultCode.Ok, paging);
		}
	}
}
