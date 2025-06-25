using Educational.Organization;
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
	[ApiExplorerSettings(GroupName ="课程")]
	public class CourseServices : ApplicationService, ICourseServices
	{
		IRepository<Course, Guid> _courseRepository;
		IRepository<OrganizationModel, Guid> organiRepository;

		public CourseServices(IRepository<Course, Guid> courseRepository, IRepository<OrganizationModel, Guid> organiRepository)
		{
			_courseRepository = courseRepository;
			this.organiRepository = organiRepository;
		}
		/// <summary>
		/// 新增课程
		/// </summary>
		/// <param name="coursedto"></param>
		/// <returns></returns>
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
		/// <summary>
		/// 获取课程分页列表
		/// </summary>
		/// <param name="seach"></param>
		/// <returns></returns>
		public async Task<ApiResult<ApiPaging<List<CourseDto>>>> GetListCourse([FromQuery]SearchCourseDto seach)
		{
			var course=await _courseRepository.GetQueryableAsync();
			if (!seach.CourseName.IsNullOrEmpty())
			{
				course= course.Where(x=>x.CourseName.Contains(seach.CourseName));
			}
			if (seach.GradeId!=null)
			{
				course= course.Where(x=>x.GratorId==seach.GradeId);
			}
			if (seach.CampusId != null)
			{
                course= course.Where(x=>x.CampusId==seach.CampusId);
			}
			if(seach.SubjectId!=null)
			{
                course= course.Where(x=>x.SubjectId==seach.SubjectId);
			}
			if (seach.Status != null)
			{
				course= course.Where(x=>x.Status==seach.Status);
			}

			var coursepage= course.Page(seach.PageIndex,seach.PageSize);
			var courselist=ObjectMapper.Map<List<Course>, List<CourseDto>>(coursepage.ToList());
			ApiPaging<List<CourseDto>> paging=new ApiPaging<List<CourseDto>>
			{
				TotleCount= course.Count(),
				Data= courselist,
                TotlePage= (int)Math.Ceiling(course.Count()* 1.0/seach.PageSize)
			};
			return ApiResult<ApiPaging<List<CourseDto>>>.Success(ResultCode.Ok, paging);
		}

		/// <summary>
		/// 批量修改课程状态
		/// </summary>
		/// <param name="id"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<ApiResult> UpdateCourseStatus(List<Guid> guids, bool status)
		{
			Guid[] ids= guids.ToArray();
			foreach(var id in ids)
			{
				var course = await _courseRepository.FirstOrDefaultAsync(x=>x.Id==id);
				course.Status = status;
                await _courseRepository.UpdateAsync(course);
			}
			return ApiResult.Success(ResultCode.Ok);
		}
	}
}
