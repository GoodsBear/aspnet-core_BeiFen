using Educational.Enums;
using Educational.Organization;
using Educational.SpecialSubject;
using Educational.Subject;
using Educational.Staffs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Educational.Classgrade;
using Educational.Dto.Grades;

namespace Educational.Courses
{
	[ApiExplorerSettings(GroupName ="课程")]
	public class CourseServices : ApplicationService, ICourseServices
	{
		IRepository<Course, Guid> _courseRepository;
		IRepository<OrganizationModel, Guid> organiRepository;
		IRepository<SubjectModel, Guid> subjectRepository;
		IRepository<SpecialSubjectModel, Guid> specialRepository;
		IRepository<OrganizationModel, Guid> organRepository;
		IRepository<Grade, Guid> gradeRepository;

		public CourseServices(IRepository<Course, Guid> courseRepository, IRepository<OrganizationModel, Guid> organiRepository, IRepository<SpecialSubjectModel, Guid> specialRepository, IRepository<SubjectModel, Guid> subjectRepository, IRepository<OrganizationModel, Guid> organRepository, IRepository<Grade, Guid> gradeRepository)
		{
			_courseRepository = courseRepository;
			this.organiRepository = organiRepository;
			this.specialRepository = specialRepository;
			this.subjectRepository = subjectRepository;
			this.organRepository = organRepository;
			this.gradeRepository = gradeRepository;
		}

		/// <summary>
		/// 新增课程
		/// </summary>
		/// <param name="coursedto"></param>
		/// <returns></returns>
		public async Task<ApiResult> AddCourse(CreateCourseDto coursedto)
		{
			try
			{
				var course = ObjectMapper.Map<CreateCourseDto, Course>(coursedto);
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
		[HttpGet("GetListCourse")]
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
			//获取科目
			var subject= ObjectMapper.Map<List<SubjectModel>,List<SubjectDto>>((await subjectRepository.GetQueryableAsync()).ToList());
			//获取专题
            var topic=ObjectMapper.Map<List<SpecialSubjectModel>,List<SpecialSubjectDto>>((await specialRepository.GetQueryableAsync()).ToList());
			//获取机构
            var organization=ObjectMapper.Map<List<OrganizationModel>,List<OrganizationDto>>((await organRepository.GetQueryableAsync()).ToList());
			//获取年级
            var grade=ObjectMapper.Map<List<Grade>,List<GradeDto>>((await gradeRepository.GetQueryableAsync()).ToList());

			var coursepage= course.Page(seach.PageIndex,seach.PageSize);
			var courselist=ObjectMapper.Map<List<Course>, List<CourseDto>>(coursepage.ToList());
			foreach (var item in courselist)
			{ 
				item.CampusName= organization.FirstOrDefault(x=>x.Id==item.CampusId)?.Name;
				item.SubjectName=subject.FirstOrDefault(x=>x.Id==item.SubjectId)?.SubjectName;
                item.TopicName=topic.FirstOrDefault(x=>x.Id==item.TopicId)?.Name;
				item.GratorName=grade.FirstOrDefault(x=>x.Id==item.GratorId)?.GradeName;
				CourseType type1 = (CourseType)item.CourseTypeId;
				item.CourseTypeName = type1.ToString();
				
			}
			ApiPaging<List<CourseDto>> paging=new ApiPaging<List<CourseDto>>
			{
				TotleCount= course.Count(),
				Data= courselist,
                TotlePage= (int)Math.Ceiling(course.Count()* 1.0/seach.PageSize)
			};
			return ApiResult<ApiPaging<List<CourseDto>>>.Success(ResultCode.Ok, paging);
		}

		/// <summary>
		/// 批量操作课程状态(上下架,启用禁用,删除)
		/// </summary>
		/// <param name="id"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<ApiResult> UpdateCourseStatus(List<Guid> guids, bool status ,int type)
		{
			Guid[] ids= guids.ToArray();
			if (type == 0)
			{
				//批量修改课程状态
				foreach(var id in ids)
				{
					
					var course = await _courseRepository.FirstOrDefaultAsync(x => x.Id == id);
					course.Status = status;
					await _courseRepository.UpdateAsync(course);
				}
				return ApiResult.Success(ResultCode.Ok);
			}else if (type == 1)
			{
				//批量上下架课程
				foreach (var id in ids)
				{
					var course = await _courseRepository.FirstOrDefaultAsync(x => x.Id == id);
					course.IsOnlineSale = status;
					await _courseRepository.UpdateAsync(course);
				}
				return ApiResult.Success(ResultCode.Ok);
			}
			else 
			{
				//批量删除课程
				foreach (var id in ids)
				{
					var course = await _courseRepository.FirstOrDefaultAsync(x => x.Id == id);
					course.IsDeleted = status;
					await _courseRepository.DeleteAsync(course);
				}
				return ApiResult.Success(ResultCode.Ok);
			}
		}

        /// <summary>
        /// 获取课程列表下拉框
        /// </summary>
        /// <returns>返回课程列表下拉框</returns>
        [HttpGet("GetCourseAsync")]
        public async Task<ApiResult<List<CourseSelectDto>>> GetCourseAsync()
        {
            try
            {
                var queryable = await _courseRepository.GetListAsync();
                var results = ObjectMapper.Map<List<Course>, List<CourseSelectDto>>(queryable);
                return ApiResult<List<CourseSelectDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "课程下拉框获取失败");
                throw;
            }
        }


    }
}
