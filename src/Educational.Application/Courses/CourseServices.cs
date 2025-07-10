using Educational.Classgrade;
using Educational.Courses.ReletedCoursedtos;
using Educational.Dto.Grades;
using Educational.Enums;
using Educational.Organization;
using Educational.SpecialSubject;
using Educational.Staffs;
using Educational.Subject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Courses
{
    [ApiExplorerSettings(GroupName = "课程")]
    public class CourseServices : ApplicationService, ICourseServices
    {
        IRepository<Course, Guid> _courseRepository;
        IRepository<OrganizationModel, Guid> organiRepository;
        IRepository<SubjectModel, Guid> subjectRepository;
        IRepository<SpecialSubjectModel, Guid> specialRepository;
        IRepository<OrganizationModel, Guid> organRepository;
        IRepository<Grade, Guid> gradeRepository;
        IRepository<ReletedCourse, Guid> courseRelationRepository;

        public CourseServices(IRepository<Course, Guid> courseRepository, IRepository<OrganizationModel, Guid> organiRepository, IRepository<SpecialSubjectModel, Guid> specialRepository, IRepository<SubjectModel, Guid> subjectRepository, IRepository<OrganizationModel, Guid> organRepository, IRepository<Grade, Guid> gradeRepository, IRepository<ReletedCourse, Guid> courseRelationRepository)
        {
            _courseRepository = courseRepository;
            this.organiRepository = organiRepository;
            this.specialRepository = specialRepository;
            this.subjectRepository = subjectRepository;
            this.organRepository = organRepository;
            this.gradeRepository = gradeRepository;
            this.courseRelationRepository = courseRelationRepository;
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
                var cour = await _courseRepository.InsertAsync(course);
                var res = cour.Equals(course);
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
        public async Task<ApiResult<ApiPaging<List<CourseDto>>>> GetListCourse([FromQuery] SearchCourseDto seach)
        {
            var course = await _courseRepository.GetQueryableAsync();
            if (!seach.CourseName.IsNullOrEmpty())
            {
                course = course.Where(x => x.CourseName.Contains(seach.CourseName));
            }
            if (seach.GradeId != null)
            {
                course = course.Where(x => x.GratorId == seach.GradeId);
            }
            if (seach.CampusId != null)
            {
                course = course.Where(x => x.CampusId == seach.CampusId);
            }
            if (seach.SubjectId != null)
            {
                course = course.Where(x => x.SubjectId == seach.SubjectId);
            }
            if (seach.Status != null)
            {
                course = course.Where(x => x.Status == seach.Status);
            }
            //获取科目
            var subject = ObjectMapper.Map<List<SubjectModel>, List<SubjectDto>>((await subjectRepository.GetQueryableAsync()).ToList());
            //获取专题
            var topic = ObjectMapper.Map<List<SpecialSubjectModel>, List<SpecialSubjectDto>>((await specialRepository.GetQueryableAsync()).ToList());
            //获取机构
            var organization = ObjectMapper.Map<List<OrganizationModel>, List<OrganizationDto>>((await organRepository.GetQueryableAsync()).ToList());
            //获取年级
            var grade = ObjectMapper.Map<List<Grade>, List<GradeDto>>((await gradeRepository.GetQueryableAsync()).ToList());

            var coursepage = course.Page(seach.PageIndex, seach.PageSize);
            var courselist = ObjectMapper.Map<List<Course>, List<CourseDto>>(coursepage.ToList());
            foreach (var item in courselist)
            {
                item.CampusName = organization.FirstOrDefault(x => x.Id == item.CampusId)?.Name;
                item.SubjectName = subject.FirstOrDefault(x => x.Id == item.SubjectId)?.SubjectName;
                item.TopicName = topic.FirstOrDefault(x => x.Id == item.TopicId)?.Name;
                item.GratorName = grade.FirstOrDefault(x => x.Id == item.GratorId)?.GradeName;
                CourseType type1 = (CourseType)item.CourseTypeId;
                item.CourseTypeName = type1.ToString();

            }
            ApiPaging<List<CourseDto>> paging = new ApiPaging<List<CourseDto>>
            {
                TotleCount = course.Count(),
                Data = courselist,
                TotlePage = (int)Math.Ceiling(course.Count() * 1.0 / seach.PageSize)
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
        public async Task<ApiResult> UpdateCourseStatus(List<Guid> guids, bool status, int type)
        {
            Guid[] ids = guids.ToArray();
            if (type == 0)
            {
                //批量修改课程状态
                foreach (var id in ids)
                {

                    var course = await _courseRepository.FirstOrDefaultAsync(x => x.Id == id);
                    course.Status = status;
                    await _courseRepository.UpdateAsync(course);
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            else if (type == 1)
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

        /// <summary>
        /// 修改课程信息
        /// </summary>
        /// <param name="coursedto"></param>
        /// <returns></returns>
        [HttpPut("UpdateCourse")]
        public async Task<ApiResult<CourseDto>> UpdateCourse(CreateUpdateCourseDto coursedto)
        {
            try
            {
                var course = await _courseRepository.GetAsync(coursedto.Id);
                if (course == null)
                {
                    return ApiResult<CourseDto>.Fail(ResultCode.Fail, "课程不存在，请检查！");
                }
                course = ObjectMapper.Map(coursedto, course);
                var updatedCourse = await _courseRepository.UpdateAsync(course);
                var result = ObjectMapper.Map<Course, CourseDto>(updatedCourse);
                return ApiResult<CourseDto>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "更新课程信息失败");
                throw;
            }
        }

        /// <summary>
        /// 批量添加课程关联
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult> AddReletedCourse(ReletedCourseDto dto)
        {
            if (dto == null || dto.CourseId == Guid.Empty || dto.Guids == null || !dto.Guids.Any())
            {
                return ApiResult.Fail(ResultCode.Fail, "参数无效");
            }

            // 查询当前课程已有关联
            var existingRelations = await _courseRepository.GetListAsync(x => x.Id == dto.CourseId);
            var existingRelatedIds = existingRelations.Select(x => x.Id).ToHashSet();

            // 过滤掉已存在的关联课程ID，只添加新的
            var newRelatedIds = dto.Guids.Where(id => !existingRelatedIds.Contains(id)).ToList();

            if (newRelatedIds.Count == 0)
            {
                return ApiResult.Success(ResultCode.Ok); // 全部已存在，视为成功
            }
            // 创建新的关联课程
            var newRelations = newRelatedIds.Select(relatedId => new ReletedCourse
            {
                Course1Id = dto.CourseId,
                Course2Id = relatedId
            }).ToList();

            await courseRelationRepository.InsertManyAsync(newRelations, autoSave: true);

            return ApiResult.Success(ResultCode.Ok);
        }
        /// <summary>
        /// 获取课程关联
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<ApiPaging<List<CourseDto>>>> GetReletedCourse(SearchReletedCourseDto search)
        {
            if (search == null || search.Id == Guid.Empty)
                return ApiResult<ApiPaging<List<CourseDto>>>.Fail(ResultCode.Fail, "参数无效");

            // 1. 查找所有与选中课程有关联的课程
            var relations = await courseRelationRepository.GetListAsync(
                x => x.Course1Id == search.Id || x.Course2Id == search.Id
            );
            if (relations.Count == 0)
                return ApiResult<ApiPaging<List<CourseDto>>>.Success(ResultCode.Ok, new ApiPaging<List<CourseDto>>
                {
                    TotleCount = 0,
                    Data = new List<CourseDto>(),
                    TotlePage = 0
                });

            // 2. 提取所有关联课程ID（去重，排除自身）
            var relatedCourseIds = relations
                .Select(r => r.Course1Id == search.Id ? r.Course2Id : r.Course1Id)
                .Where(id => id != search.Id)
                .ToHashSet();

            // 3. 查找这些课程的详细信息
            var relatedCourses = await _courseRepository.GetListAsync(x => relatedCourseIds.Contains(x.Id));

            // 4. 映射为CourseDto
            var result = relatedCourses.Select(x => ObjectMapper.Map<Course, CourseDto>(x)).ToList();
            //获取科目
            var subject = ObjectMapper.Map<List<SubjectModel>, List<SubjectDto>>((await subjectRepository.GetQueryableAsync()).ToList());
            //获取专题
            var topic = ObjectMapper.Map<List<SpecialSubjectModel>, List<SpecialSubjectDto>>((await specialRepository.GetQueryableAsync()).ToList());
            //获取机构
            var organization = ObjectMapper.Map<List<OrganizationModel>, List<OrganizationDto>>((await organRepository.GetQueryableAsync()).ToList());
            //获取年级
            var grade = ObjectMapper.Map<List<Grade>, List<GradeDto>>((await gradeRepository.GetQueryableAsync()).ToList());



            // 5. 分页
            int totalCount = result.Count;
            int pageIndex = search.PageIndex > 0 ? search.PageIndex : 1;
            int pageSize = search.PageSize > 0 ? search.PageSize : 10;
            var pageData = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            int totalPage = (int)Math.Ceiling(totalCount * 1.0 / pageSize);
            foreach (var item in pageData)
            {
                item.CampusName = organization.FirstOrDefault(x => x.Id == item.CampusId)?.Name;
                item.SubjectName = subject.FirstOrDefault(x => x.Id == item.SubjectId)?.SubjectName;
                item.TopicName = topic.FirstOrDefault(x => x.Id == item.TopicId)?.Name;
                item.GratorName = grade.FirstOrDefault(x => x.Id == item.GratorId)?.GradeName;
                CourseType type1 = (CourseType)item.CourseTypeId;
                item.CourseTypeName = type1.ToString();

            }
            var paging = new ApiPaging<List<CourseDto>>
            {
                TotleCount = totalCount,
                Data = pageData,
                TotlePage = totalPage
            };

            return ApiResult<ApiPaging<List<CourseDto>>>.Success(ResultCode.Ok, paging);
        }
        /// <summary>
        /// 移除课程关联
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult> RemoveReletedCourse(Guid guid, Guid id)
        {
            // 1. 查找关联关系
            var relation = await courseRelationRepository.FirstOrDefaultAsync(x => x.Course1Id == id && x.Course2Id == guid);
            if (relation == null)
                return ApiResult.Success(ResultCode.Ok);
            // 3. 移除
            await courseRelationRepository.DeleteAsync(relation);
            // 4. 返回
            return ApiResult.Success(ResultCode.Ok);
        }
    }

}
