using Educational.Classgrade;
using Educational.ClassRooms;
using Educational.Courses;
using Educational.Dto.ClassRooms;
using Educational.Dto.Clbums;
using Educational.Dto.Grades;
using Educational.Enums;
using Educational.Materials;
using Educational.Organization;
using Educational.Staffs;
using Educational.Subject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUglify.JavaScript.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Educational.Clbums
{
    [ApiExplorerSettings(GroupName = "班级")]
    public class ClassInfoAppService : ApplicationService, IClassInfoAppService
    {
        IRepository<ClassInfo, Guid> classinfoRep;
        IRepository<Educational.Classgrade.ClassRoom, Guid> classRoomRep;
        IRepository<Grade, Guid> gradeRep;
        IRepository<StaffInfo, Guid> stafffoRep;
        IRepository<OrganizationModel, Guid> organizationRep;
        IRepository<Course, Guid> courseRep;
        ILogger<ClassInfoAppService> logger;
        IRepository<SubjectModel,Guid> subjectModelRep;
        public ClassInfoAppService(IRepository<ClassInfo, Guid> classinfoRep, IRepository<Educational.Classgrade.ClassRoom, Guid> classRoomRep, IRepository<Grade, Guid> gradeRep, IRepository<StaffInfo, Guid> stafffoRep, IRepository<OrganizationModel, Guid> organizationRep, IRepository<Course, Guid> courseRep, ILogger<ClassInfoAppService> logger, IRepository<SubjectModel, Guid> subjectModelRep)
        {
            this.classinfoRep = classinfoRep;
            this.classRoomRep = classRoomRep;
            this.gradeRep = gradeRep;
            this.stafffoRep = stafffoRep;
            this.organizationRep = organizationRep;
            this.courseRep = courseRep;
            this.logger = logger;
            this.subjectModelRep = subjectModelRep;
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
                createUpdateClassDto.ClassStatus = LessonStateEnum.未排课;
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
                var classList = await classinfoRep.GetQueryableAsync();
                var gradeList = await gradeRep.GetQueryableAsync();
                var staffList = await stafffoRep.GetQueryableAsync();
                var orgList = await organizationRep.GetQueryableAsync();
                var courseList = await courseRep.GetQueryableAsync();
                var subjectList = await subjectModelRep.GetQueryableAsync();
                var classRoomList = await classRoomRep.GetQueryableAsync();
                classList = classList.WhereIf(!string.IsNullOrEmpty(searchDto.ClassName), x => x.ClassName.Contains(searchDto.ClassName));
                classList = classList.WhereIf(searchDto.CampusId != null, x => x.CampusId == searchDto.CampusId);
                classList = classList.WhereIf(searchDto.DefaultCourseId != null, x => x.DefaultCourseId == searchDto.DefaultCourseId);
                classList = classList.WhereIf(searchDto.DefaultClassroomId != null, x => x.DefaultClassroomId == searchDto.DefaultClassroomId);
                classList = classList.WhereIf(searchDto.ClassTeacherId != null, x => x.ClassTeacherId == searchDto.ClassTeacherId);
                classList = classList.WhereIf(searchDto.GradeId != null, x => x.GradeId == searchDto.GradeId);
                classList = classList.WhereIf(searchDto.ClassStatus != null, x => x.ClassStatus == (LessonStateEnum)searchDto.ClassStatus);
                var page = classList.PageResult(searchDto.PageIndex, searchDto.PageSize);
                var classInfoDto = ObjectMapper.Map<List<ClassInfo>, List<ClassInfoDto>>(page.Queryable.ToList());
                foreach (var item in classInfoDto)
                {
                    item.Name = orgList.FirstOrDefault(x => x.Id == item.CampusId).Name;
                    item.GradeName = gradeList.FirstOrDefault(x => x.Id == item.GradeId).GradeName;
                    item.StaffName = staffList.FirstOrDefault(x => x.Id == item.ClassTeacherId).StaffName;
                    item.ClassRoomName = classRoomList.FirstOrDefault(x => x.Id == item.DefaultClassroomId).ClassRoomName;
                    item.CourseName = courseList.FirstOrDefault(x => x.Id == item.DefaultCourseId).CourseName;
                    item.LessonNum = courseList.FirstOrDefault(x => x.Id == item.DefaultCourseId).LessonNum;
                    item.SubjectId = courseList.FirstOrDefault(x => x.Id == item.DefaultCourseId).SubjectId;
                    item.SubjectName = subjectList.FirstOrDefault(x => x.Id == item.SubjectId).SubjectName;
                    item.ClassStatusName = Enum.GetName(typeof(LessonStateEnum), item.ClassStatus);
                }
                var paging = new ApiPaging<List<ClassInfoDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / searchDto.PageSize),
                    Data = classInfoDto
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
        /// <summary>
        /// 班级反填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<ClassInfoDto>> FTClassInfo(Guid id)
        {
            try
            {
                var classList = await classinfoRep.GetQueryableAsync();
                var gradeList = await gradeRep.GetQueryableAsync();
                var staffList = await stafffoRep.GetQueryableAsync();
                var orgList = await organizationRep.GetQueryableAsync();
                var courseList = await courseRep.GetQueryableAsync();
                var classRoomList = await classRoomRep.GetQueryableAsync();
                var subjectList = await subjectModelRep.GetQueryableAsync();
                var list = await classinfoRep.FirstOrDefaultAsync(x => x.Id == id);
                var listdto = ObjectMapper.Map<ClassInfo, ClassInfoDto>(list);
                listdto.Name = orgList.FirstOrDefault(x => x.Id == listdto.CampusId).Name;
                listdto.GradeName = gradeList.FirstOrDefault(x => x.Id == listdto.GradeId).GradeName;
                listdto.StaffName = staffList.FirstOrDefault(x => x.Id == listdto.ClassTeacherId).StaffName;
                listdto.ClassRoomName = classRoomList.FirstOrDefault(x => x.Id == listdto.DefaultClassroomId).ClassRoomName;
                listdto.CourseName = courseList.FirstOrDefault(x => x.Id == listdto.DefaultCourseId).CourseName;
                listdto.LessonNum = courseList.FirstOrDefault(x => x.Id == listdto.DefaultCourseId).LessonNum;
                listdto.SubjectId = courseList.FirstOrDefault(x => x.Id == listdto.DefaultCourseId).SubjectId;
                listdto.SubjectName = subjectList.FirstOrDefault(x => x.Id == listdto.SubjectId).SubjectName;
                listdto.ClassStatusName = Enum.GetName(typeof(LessonStateEnum), listdto.ClassStatus);
            
                return ApiResult<ClassInfoDto>.Success(ResultCode.Ok, listdto);
            }
            catch (Exception ex)
            {
                logger.LogError("获取班级信息出错！" + ex.Message);
                throw;
            }
        }
    }
}
