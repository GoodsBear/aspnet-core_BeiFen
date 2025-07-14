using Educational.Classgrade;
using Educational.Courses;
using Educational.Dto.MaterialRecordsDtos;
using Educational.Enums;
using Educational.Materials;
using Educational.Organization;
using Educational.Staffs;
using Educational.StudentsAndParends.Parents;
using Educational.StudentsAndParends.Students;
using Educational.StudentsAndParends.Students.EnrollmentRecords;
using Educational.StudentsAndParents.EnrollmentRecords;
using Educational.StudentsAndParents.Stores;
using Educational.StudentsAndParents.StudentFollow;
using Educational.Subject;
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
using Volo.Abp.ObjectMapping;

namespace Educational.StudentsAndParents.StudentServices
{
    [ApiExplorerSettings(GroupName = "报名记录")]
    public class EnrollmentRecordService: ApplicationService, IEnrollmentRecordService
    {
        public  readonly IRepository<EnrollmentRecord, Guid> enrollmentRep;
        private readonly IRepository<Student, Guid> studentRep;
        private readonly IRepository<Parent, Guid> parentRep;
        private readonly IRepository<Grade, Guid> gardRep;
        private readonly IRepository<StaffInfo, Guid> staffRep;
        private readonly IRepository<Course, Guid> courseRep;
        private readonly IRepository<SubjectModel, Guid> subjectRep;
        private readonly IRepository<OrganizationModel> organizationRep;
        private readonly ILogger<StudentServices> logger;
        public EnrollmentRecordService(IRepository<EnrollmentRecord, Guid> enrollmentRep, IRepository<Student, Guid> studentRep, IRepository<Parent, Guid> parentRep, IRepository<Grade, Guid> gardRep, IRepository<StaffInfo, Guid> staffRep, IRepository<Course, Guid> courseRep, IRepository<OrganizationModel> organizationRep, ILogger<StudentServices> logger, IRepository<SubjectModel, Guid> subjectRep)
        {
            this.enrollmentRep = enrollmentRep;
            this.studentRep = studentRep;
            this.parentRep = parentRep;
            this.gardRep = gardRep;
            this.staffRep = staffRep;
            this.courseRep = courseRep;
            this.organizationRep = organizationRep;
            this.logger = logger;
            this.subjectRep = subjectRep;
        }
        /// <summary>
        /// 报名记录的添加
        /// </summary>
        /// <param name="createUpdateEnrollmentRecordDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<EnrollmentRecordDto>> AddEnrollmentAsync(CreateUpdateEnrollmentRecordDto createUpdateEnrollmentRecordDto)
        {
            try
            {
                createUpdateEnrollmentRecordDto.EnrollmentDate = DateTime.Now;
                createUpdateEnrollmentRecordDto.ExamineStatus = Examine.审核中;
                var createUpdateEnrollmentRecord = ObjectMapper.Map<CreateUpdateEnrollmentRecordDto, EnrollmentRecord>(createUpdateEnrollmentRecordDto);
                var result = await enrollmentRep.InsertAsync(createUpdateEnrollmentRecord);
                return ApiResult<EnrollmentRecordDto>.Success(ResultCode.Ok, ObjectMapper.Map<EnrollmentRecord, EnrollmentRecordDto>(result));

            }
            catch (Exception ex)
            {
                logger.LogError("报名记录添加失败", ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// 批量报名
        /// </summary>
        public async Task<ApiResult<List<EnrollmentRecordDto>>> BatchAddEnrollmentAsync(List<CreateUpdateEnrollmentRecordDto> dtos)
        {
            var resultList = new List<EnrollmentRecordDto>();
            try
            {
                foreach (var dto in dtos)
                {
                    var student = await studentRep.FirstOrDefaultAsync(x => x.Id == dto.StudentId);
                    var course = await courseRep.FirstOrDefaultAsync(x => x.Id == dto.CourseId);
                    dto.EnrollmentDate = DateTime.Now;
                    dto.ExamineStatus = Examine.审核中;
                    var entity = ObjectMapper.Map<CreateUpdateEnrollmentRecordDto, EnrollmentRecord>(dto);
                    var result = await enrollmentRep.InsertAsync(entity);
                    resultList.Add(ObjectMapper.Map<EnrollmentRecord, EnrollmentRecordDto>(result));
                }
                return ApiResult<List<EnrollmentRecordDto>>.Success(ResultCode.Ok, resultList);
            }
            catch (Exception ex)
            {
                logger.LogError("批量报名失败", ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 报名列表
        /// </summary>
        /// <param name="seachDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<ApiPaging<List<EnrollmentRecordDto>>>> GetEnrollmentRecordList([FromQuery]EnrollmentSeachDto seachDto)
        {
            try
            {
                var studentList = await studentRep.GetQueryableAsync();
                var couserList = await courseRep.GetQueryableAsync();
                var enrollmentList = await enrollmentRep.GetQueryableAsync();
                var staffList = await staffRep.GetQueryableAsync();
                var subjectList = await subjectRep.GetQueryableAsync();

                var list = from enrollment in enrollmentList
                           join student in studentList on enrollment.StudentId equals student.Id
                           join couser in couserList on enrollment.CourseId equals couser.Id
                           join staffinfo in staffList on student.Consultant equals staffinfo.Id
                           join subject in subjectList on couser.SubjectId equals subject.Id
                           select new EnrollmentRecordDto
                           {
                               Id = enrollment.Id,
                               CourseId = enrollment.CourseId,
                               CourseName = couser.CourseName,
                               SubjectId = couser.SubjectId,
                               SubjectName = subject.SubjectName,
                               StudentId = enrollment.StudentId,
                               Name = student.Name,
                               EnrollmentDate = enrollment.EnrollmentDate,
                               RecordType = enrollment.RecordType,
                               StartTime = enrollment.StartTime,
                               ValidTime = enrollment.ValidTime,
                               BugLessons = enrollment.BugLessons,
                               LessonPrice = enrollment.LessonPrice,
                               FactGetPrice = enrollment.FactGetPrice,
                               DisCountPrice = enrollment.DisCountPrice,
                               StaffId = enrollment.StaffId,
                               StaffName = staffinfo.StaffName,
                               RefundMoney = enrollment.RefundMoney,
                               RefundNumber=enrollment.RefundNumber,
                               RefundDesc=enrollment.RefundDesc,

                           };
                foreach (var item in list)
                {
                    Examine type1 = (Examine)item.RecordType;
                    item.RecordTypeName = type1.ToString();
                    
                }
                // 条件过滤
                if (!string.IsNullOrEmpty(seachDto.Name))
                {

                    list = list.Where(x => x.Name.Contains(seachDto.Name));
                }
                if (!string.IsNullOrEmpty(seachDto.StartTime))
                {
                    list = list.Where(x => x.StartTime == DateTime.Parse(seachDto.StartTime));
                }
                if (!string.IsNullOrEmpty(seachDto.EndTime))
                {
                    list = list.Where(x => x.ValidTime == DateTime.Parse(seachDto.EndTime));
                }
                if (seachDto.CourseId != null)
                {
                    list = list.Where(x => x.CourseId == seachDto.CourseId);
                }
                if (seachDto.Consultant != null)
                {
                    list = list.Where(x => x.StaffId == seachDto.Consultant);
                }
                var paging = list.PageResult(seachDto.PageIndex, seachDto.PageSize);
                var result = new ApiPaging<List<EnrollmentRecordDto>>
                {
                    TotleCount = paging.RowCount,
                    TotlePage = (int)Math.Ceiling(paging.RowCount * 1.0 / seachDto.PageSize),
                    Data = paging.Queryable.ToList()
                };
                return ApiResult<ApiPaging<List<EnrollmentRecordDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("报名记录列表出错！", ex.Message);
                throw;
            }
        }
    }
}
