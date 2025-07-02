using Educational.Classgrade;
using Educational.ClassRooms;
using Educational.Courses;
using Educational.Dto.Clbums;
using Educational.Dto.Grades;
using Educational.Enums;
using Educational.Organization;
using Educational.Staffs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUglify.JavaScript.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Clbums
{
    [ApiExplorerSettings(GroupName = "班级")]
    public class ClassInfoAppService : ApplicationService, IClassInfoAppService
    {
        IRepository<ClassInfo, Guid> classinfoRep;
        IRepository<ClassRoom, Guid> classRoomRep;
        IRepository<Grade, Guid> gradeRep;
        IRepository<StaffInfo, Guid> stafffoRep;
        IRepository<OrganizationModel, Guid> organizationRep;
        IRepository<Course, Guid> courseRep;
        ILogger<ClassInfoAppService> logger;

        public ClassInfoAppService(IRepository<ClassInfo, Guid> classinfoRep, IRepository<ClassRoom, Guid> classRoomRep, IRepository<Grade, Guid> gradeRep, IRepository<StaffInfo, Guid> stafffoRep, IRepository<OrganizationModel, Guid> organizationRep, IRepository<Course, Guid> courseRep, ILogger<ClassInfoAppService> logger)
        {
            this.classinfoRep = classinfoRep;
            this.classRoomRep = classRoomRep;
            this.gradeRep = gradeRep;
            this.stafffoRep = stafffoRep;
            this.organizationRep = organizationRep;
            this.courseRep = courseRep;
            this.logger = logger;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"> 数组id</param>
        /// <returns>返回受影响行数</returns>
        [HttpDelete]
        public async Task<ApiResult> BatchDelete(List<Guid> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var classInfo = await classinfoRep.GetAsync(item);
                    if (classInfo == null)
                    {
                        return ApiResult.Fail(ResultCode.Fail, "班级不存在！");
                    }
                    await classinfoRep.DeleteAsync(classInfo);

                }
                return ApiResult.Success(ResultCode.Ok);

            }
            catch (Exception ex)
            {
                logger.LogError("班级批量删除出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 创建班级
        /// </summary>
        /// <param name="createUpdateClassDto">班级dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult<ClassInfoDto>> CreateClass(CreateUpdateClassDto createUpdateClassDto)
        {
            try
            {

                var classInfo = ObjectMapper.Map<CreateUpdateClassDto, ClassInfo>(createUpdateClassDto);
                var result = await classinfoRep.InsertAsync(classInfo);
                var classInfodto = ObjectMapper.Map<ClassInfo, ClassInfoDto>(result);
                return ApiResult<ClassInfoDto>.Success(ResultCode.Ok, classInfodto);
            }
            catch (Exception ex)
            {
                logger.LogError("班级添加出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 班级下拉框
        /// </summary>
        /// <returns>返回班级下拉框</returns>
        public async Task<ApiResult<List<ClassSelectDto>>> GetClassAsync()
        {
            try
            {
                var queryable = await classinfoRep.GetListAsync();
                var results = ObjectMapper.Map<List<ClassInfo>, List<ClassSelectDto>>(queryable);
                return ApiResult<List<ClassSelectDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "班级下拉框获取失败");
                throw;
            }
        }
        /// <summary>
        /// 班级列表
        /// </summary>
        /// <param name="searchDto">班级搜索dto</param>
        /// <returns>返回班级列表</returns>
        public async Task<ApiResult<ApiPaging<List<ClassInfoDto>>>> GetClassList([FromQuery] ClassSearchDto searchDto)
        {
            try
            {
                var classList = await classinfoRep.GetListAsync();
                var gradeList = await gradeRep.GetListAsync();
                var staffList = await stafffoRep.GetListAsync();
                var orgList = await organizationRep.GetListAsync();
                var courseList = await courseRep.GetListAsync();
                var classRoomList = await classRoomRep.GetListAsync();
                var queryable = classList.AsQueryable();
                queryable = queryable.WhereIf(!string.IsNullOrEmpty(searchDto.ClassName), x => x.ClassName.Contains(searchDto.ClassName));
                queryable = queryable.WhereIf(searchDto.CampusId != null, x => x.CampusId == searchDto.CampusId);
                queryable = queryable.WhereIf(searchDto.DefaultCourseId != null, x => x.DefaultCourseId == searchDto.DefaultCourseId);
                queryable = queryable.WhereIf(searchDto.DefaultClassroomId != null, x => x.DefaultClassroomId == searchDto.DefaultClassroomId);
                queryable = queryable.WhereIf(searchDto.ClassTeacherId != null, x => x.ClassTeacherId == searchDto.ClassTeacherId);
                queryable = queryable.WhereIf(searchDto.GradeId != null, x => x.GradeId == searchDto.GradeId);
                queryable = queryable.WhereIf(searchDto.ClassStatus != null, x => x.ClassStatus == (LessonStateEnum)searchDto.ClassStatus);
                // 联查
                var list = (from classinfo in queryable
                              join grade in gradeList on classinfo.GradeId equals grade.Id
                              join staff in staffList on classinfo.ClassTeacherId equals staff.Id
                              join organization in orgList on classinfo.CampusId equals organization.Id
                              join course in courseList on classinfo.DefaultCourseId equals course.Id
                              join classroom in classRoomList on classinfo.DefaultClassroomId equals classroom.Id
                              select new ClassInfoDto
                              {
                                  ClassName = classinfo.ClassName,
                                  CampusId = classinfo.CampusId,
                                  Name = organization.Name,
                                  GradeId = classinfo.GradeId,
                                  GradeName = grade.GradeName,
                                  ClassTeacherId = classinfo.ClassTeacherId,
                                  StaffName = staff.StaffName,
                                  PreNum = classinfo.PreNum,
                                  PreCourseNum = classinfo.PreCourseNum,
                                  DefaultCourseId = classinfo.DefaultCourseId,
                                  CourseName = course.CourseName,
                                  DefaultClassroomId = classinfo.DefaultClassroomId,
                                  ClassRoomName = classroom.ClassRoomName,
                                  PlanOpenDate = classinfo.PlanOpenDate,
                                  PlanCloseDate = classinfo.PlanCloseDate,
                                  ClassQrCode = classinfo.ClassQrCode,
                                  CourseRemark = classinfo.CourseRemark,
                                  ClassStatus = classinfo.ClassStatus,
                              }).ToList();
                int total = list.Count();
                var pageList = list
                    .Skip((searchDto.PageIndex - 1) * searchDto.PageSize)
                    .Take(searchDto.PageSize)
                    .ToList();
                var paging = new ApiPaging<List<ClassInfoDto>>
                {
                    TotleCount = total,
                    TotlePage = (int)Math.Ceiling(total * 1.0 / searchDto.PageSize),
                    Data = pageList
                };
                return ApiResult<ApiPaging<List<ClassInfoDto>>>.Success(ResultCode.Ok, paging);
            }
            catch (Exception ex)
            {
                logger.LogError("班级显示报错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 修改班级
        /// </summary>
        /// <param name="id">根据id查询</param>
        /// <param name="createUpdateClassDto">班级修改dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult<ClassInfoDto>> UpdateClass(Guid id, CreateUpdateClassDto createUpdateClassDto)
        {
            try
            {
                var classinfo = await classinfoRep.GetAsync(id);
                if (classinfo == null)
                {
                    return ApiResult<ClassInfoDto>.Fail(ResultCode.Fail, "班级不存在！");
                }
                var result = ObjectMapper.Map(createUpdateClassDto, classinfo);
                await classinfoRep.UpdateAsync(result);
                return ApiResult<ClassInfoDto>.Success(ResultCode.Ok, ObjectMapper.Map<ClassInfo, ClassInfoDto>(result));
            }
            catch (Exception ex)
            {
                logger.LogError("班级修改出错！" + ex.Message);
                throw;
            }
        }
        ///// <summary>
        ///// 修改班级状态
        ///// </summary>
        ///// <param name="id">班级ID</param>
        ///// <param name="Status">新状态（枚举值）</param>
        ///// <returns>返回受影响行数</returns>
        //[HttpPost]
        //public async Task<ApiResult> UpdateClassStatus(Guid id, int Status)
        //{
        //    try
        //    {
        //        var classInfo = await classinfoRep.GetAsync(id);
        //        if (classInfo == null)
        //        {
        //            return ApiResult.Fail(ResultCode.Fail, "班级不存在！");
        //        }
        //        classInfo.ClassStatus = (LessonStateEnum)Status;
        //        await classinfoRep.UpdateAsync(classInfo);
        //        return ApiResult.Success(ResultCode.Ok);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("班级状态修改出错！" + ex.Message);
        //        throw;
        //    }
        //}
        /// <summary>
        /// 批量修改班级状态
        /// </summary>
        /// <param name="ids">班级ID列表</param>
        /// <param name="Status">目标状态（枚举int值）</param>
        /// <returns>返回受影响行数</returns>
        [HttpPut]
        public async Task<ApiResult> BatchUpdateClassStatus(List<Guid> ids)
        {
            try
            {
                int successCount = 0;
                foreach (var id in ids)
                {
                    var classInfo = await classinfoRep.GetAsync(id);
                    if (classInfo != null)
                    {
                        classInfo.ClassStatus = LessonStateEnum.已结业;
                        await classinfoRep.UpdateAsync(classInfo);
                        successCount++;
                    }
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("批量修改班级状态出错！" + ex.Message);
                throw;
            }
        }

        public   Task<ApiResult> UpdateClassStatus(Guid id, int Status)
        {
            throw new NotImplementedException();
        }
    }
}
