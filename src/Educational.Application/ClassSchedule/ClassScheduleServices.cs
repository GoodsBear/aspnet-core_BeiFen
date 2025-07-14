using Educational.Classgrade;
using Educational.ClassSchedule.DTO;
using Educational.ClassSchedule.Update;
using Educational.Courses;
using Educational.Enums;
using Educational.Organization;
using Educational.SalarySetting;
using Educational.Staffs;
using Educational.StudentsAndParends.Students;
using Educational.Subject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Educational.ClassSchedule
{
    [ApiExplorerSettings(GroupName = "排课")]
    public class ClassScheduleServices : ApplicationService, IClassScheduleServices
    {
        IRepository<ClassHourFeeSetting, Guid> _classHourFeeSettingRepository;//上课薪资设置
        IRepository<SalarySettingModel, Guid> _salarySettingRepository;//薪资
        IRepository<ClassSchedule, Guid> _classScheduleRepository;//排课
        IRepository<ScheduleTime, Guid> _scheduleTimeRepository;//上课时间
        IRepository<ConflictModel, Guid> _conflictModelRepository;//冲突
        IRepository<ClassInfo, Guid> _classInfoRepository;//班级
        IRepository<Educational.Classgrade.ClassRoom, Guid> _classRoomRepository;//教室号
        IRepository<Course, Guid> _courseRepository;//课程
        IRepository<StaffInfo, Guid> _staffInfoRepository;//职工
        IRepository<OrganizationModel, Guid> _organizationRepository;//组织机构
        IRepository<Student, Guid> _studentRepository;//学生表 
        ILogger<ClassScheduleServices> _logger;
        public ClassScheduleServices(IRepository<ClassSchedule, Guid> classScheduleRepository, IRepository<ScheduleTime, Guid> scheduleTimeRepository, IRepository<ConflictModel, Guid> conflictModelRepository, IRepository<ClassInfo, Guid> classInfoRepository, IRepository<Course, Guid> courseRepository, IRepository<StaffInfo, Guid> staffInfoRepository, IRepository<OrganizationModel, Guid> organizationRepository, ILogger<ClassScheduleServices> logger, IRepository<Student, Guid> studentRepository, IRepository<SalarySettingModel, Guid> salarySettingRepository, IRepository<ClassHourFeeSetting, Guid> classHourFeeSettingRepository, IRepository<ClassRoom, Guid> classRoomRepository)
        {
            _organizationRepository = organizationRepository;
            _classScheduleRepository = classScheduleRepository;
            _scheduleTimeRepository = scheduleTimeRepository;
            _conflictModelRepository = conflictModelRepository;
            _classInfoRepository = classInfoRepository;
            _courseRepository = courseRepository;
            _staffInfoRepository = staffInfoRepository;
            _studentRepository = studentRepository;
            _logger = logger;
            _salarySettingRepository = salarySettingRepository;
            _classHourFeeSettingRepository = classHourFeeSettingRepository;
            _classRoomRepository = classRoomRepository;
        }

        //显示冲突表
        public async Task<ApiResult<ApiPaging<List<ConflictModelDto>>>> GetConfilcListAsync([FromQuery] Seach search)
        {
            try
            {
                // 构建查询
                var conflictlist = await _conflictModelRepository.GetQueryableAsync();
                // 使用ABP自带分页方法 
                var page = conflictlist.PageResult(search.PageIndex, search.PageSize);
                // 映射 
                var linq = ObjectMapper.Map<List<ConflictModel>, List<ConflictModelDto>>(page.Queryable.ToList());
                var result = new ApiPaging<List<ConflictModelDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / search.PageSize),
                    Data = linq.ToList()
                };
                return ApiResult<ApiPaging<List<ConflictModelDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                Logger.LogError("显示冲突-异常" + ex);
                return ApiResult<ApiPaging<List<ConflictModelDto>>>.Fail(ResultCode.Fail, $"显示冲突--异常: {ex.Message}");
            }
        }
        //逻辑删除
        /// <summary>
        /// 添加排课表----排课表的生成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult> CreateClassScheduleAsync(UpdateClassScheduleDto input)
        {
            //添加检查
            await AddCheck(input);
            try
            {
                //开始添加 排课表 
                var Campus = await _organizationRepository.FirstOrDefaultAsync(x => x.Name == input.OrganizationName);//组织
                if (Campus == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "未找到指定的校区");
                }
                var Class = await _classInfoRepository.FirstOrDefaultAsync(x => x.ClassName == input.ClassName);//班级
                if (Class == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "未找到指定的班级");
                }
                var Course = await _courseRepository.FirstOrDefaultAsync(x => x.CourseName == input.CourseName);//课程   
                if (Course == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "未找到指定的课程");
                }
                //老师list<string> 
                var ClassSchedules = ObjectMapper.Map<UpdateClassScheduleDto, Educational.ClassSchedule.ClassSchedule>(input);
                ClassSchedules.ClassId = Class.Id;
                ClassSchedules.OrganizationId = Campus.Id;
                ClassSchedules.CourseId = Course.Id; 
                ClassSchedules.MainTeacher = input.MainTeacher; 
                ClassSchedules.AssistantTeacher = input.AssistantTeacher;
                var schedule = await _classScheduleRepository.InsertAsync(ClassSchedules);
                //上课时间表
                var map = ObjectMapper.Map<List<UpdateScheduleTime>, List<ScheduleTime>>(input.ScheduleTimes);
                //给List<ScheduleTime>中的每个ScheduleTime设置ClassScheduleId
                map.ForEach(time => time.ClassScheduleId = schedule.Id);
                var classroom = await _classRoomRepository.GetQueryableAsync();
                foreach (var item in map)
                {
                    classroom = classroom.Where(x => x.Id == item.ClassroomId);
                    item.ClassroomName = classroom.FirstOrDefault().ClassRoomName;
                }
                //优化后的，统一获取教室号。（报错但是觉得有用注释了不要删/cs）
                //var classRoomIds = map.Select(x => x.ClassroomId).Distinct().ToList();   
                //var classrooms = await _classRoomRepository.GetListAsync(x => classRoomIds.Contains(x.Id));
                //var roomDict = classrooms.ToDictionary(r => r.Id, r => r.ClassRoomName);

                //foreach (var item in map)
                //{
                //    if (roomDict.TryGetValue(item.ClassroomId, out var name))
                //    {
                //        item.ClassroomName = name;
                //    }
                //}
                await _scheduleTimeRepository.InsertManyAsync(map);
                //返回
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail(ResultCode.Fail, $"创建排课失败: {ex.Message}");
            }
        }
        /// <summary>
        /// 分页查询排课表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<ApiResult<ApiPaging<List<ClassScheduleDto>>>> GetListAsync([FromQuery] ClassScheduleSearchDto search)
        {
            try
            {
                // 构建查询
                var classSchedulelist = await _classScheduleRepository.GetQueryableAsync();
                var scheduleTimeSub = await _scheduleTimeRepository.GetQueryableAsync(); 
                //  查询
                classSchedulelist = classSchedulelist.WhereIf(search.organizationId != null, x => x.OrganizationId.Equals(search.organizationId));
                classSchedulelist = classSchedulelist.WhereIf(search.ClassId != null, x => x.ClassId.Equals(search.ClassId));
                classSchedulelist = classSchedulelist.WhereIf(search.CourseName != null, x => x.CourseName.Contains(search.CourseName));
                var linq = from s in classSchedulelist
                           join t in scheduleTimeSub on s.Id equals t.ClassScheduleId
                           select new ClassScheduleDto
                           {
                               Id=s.Id,
                               IsTimetableGenerated = s.IsTimetableGenerated,
                               HasSchedulingConflict = s.HasSchedulingConflict,
                               OrganizationName = s.OrganizationName,
                               ClassName = s.ClassName,
                               CourseName = s.CourseName,
                               MainTeacher = s.MainTeacher,
                               AssistantTeacher = s.AssistantTeacher,
                               StartDate = s.StartDate,
                               EndDate = s.EndDate,
                               MaxSchedules = s.MaxSchedules,
                               GeneratedSessionCount = s.GeneratedSessionCount,
                               MaxAttendees = s.MaxAttendees, 
                               ConsumptionBase = s.ConsumptionBase,
                               SkipHolidays = s.SkipHolidays,
                               //  ScheduleTimes = scheduleTimeSub.ToString()
                               ScheduleTimes = $"{t.DayOfWeek} {t.StartTime:hh\\:mm}-{t.EndTime:hh\\:mm}" 
                           }; 
                // 使用ABP自带分页方法 
                var page = linq.PageResult(search.PageIndex, search.PageSize);
                // 映射 
                //var subjectDto = ObjectMapper.Map<List<Educational.Subject.SubjectModel>, List<SubjectDto>>(page.Queryable.ToList());
                var result = new ApiPaging<List<ClassScheduleDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / search.PageSize),
                    Data = linq.ToList()
                };
                return ApiResult<ApiPaging<List<ClassScheduleDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                Logger.LogError("显示排课--异常" + ex);
                return ApiResult<ApiPaging<List<ClassScheduleDto>>>.Fail(ResultCode.Fail, $"查询排课分页--异常: {ex.Message}");
            }
        }
        //反填
        public async Task<ApiResult<ClassSchedule>> GetOneAsync(Guid id)
        {
            try
            {
                var course = await _classScheduleRepository.FirstOrDefaultAsync(x => x.Id == id);
                return ApiResult<ClassSchedule>.Success(ResultCode.Ok, course);
            }
            catch (Exception ex)
            {
                _logger.LogError("反填排课计划失败" + ex);
                return ApiResult<ClassSchedule>.Fail(ResultCode.Fail, $"反填---排课异常: {ex.Message}");
            }
        }
        //修改排课 
        public async Task<ApiResult> UpdateClassScheduleAsync(Guid id, ClassSchedule input)
        {
            try
            {
                // 检查排课是否存在
                var organization = await _classScheduleRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (organization == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "排课不存在");
                }
                ObjectMapper.Map(input, organization);
                //organization.Id = id;
                //result.Id = id;
                var a = await _classScheduleRepository.UpdateAsync(organization);
                if (a == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "更新排课失败");
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新排课失败" + ex);
                return ApiResult<Educational.Subject.SubjectModel>.Fail(ResultCode.Fail, $"更新排课失败: {ex.Message}");
            }
        }
        //批量删除
        public async Task<ApiResult> DeletedClassAsync(List<Guid> guids)
        {
            try
            {
                Guid[] ids = guids.ToArray();

                foreach (var id in ids)
                {
                    var course = await _classScheduleRepository.FirstOrDefaultAsync(x => x.Id == id);
                    //删除
                    await _classScheduleRepository.DeleteAsync(course);
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError("批量删除排课计划失败" + ex);
                return ApiResult<SubjectDto>.Fail(ResultCode.Fail, $"批量删除排课计划失败: {ex.Message}");
            }
        }

        public Task<ApiResult> CheckConflict(ClassScheduleDto dto)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加排课检查
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddCheck(UpdateClassScheduleDto input)
        {
            try
            {
                //日期设置检查
                if (input.StartDate > input.EndDate)
                {
                    return ApiResult<ClassScheduleDto>.Fail(ResultCode.Fail, "日期设置错误，开始日期大于结束日期");
                }
                //上课老师和助教老师不能重复 
                // 如果助教列表为空（或为null），则不可能有重复
                if (input.AssistantTeacher != null || input.AssistantTeacher.Any())
                {
                    //Intersect方法，它会返回两个序列的交集
                    var a = input.MainTeacher.Intersect(input.AssistantTeacher).Any();
                    if (a)
                    {
                        return ApiResult<ClassScheduleDto>.Fail(ResultCode.Fail, "上课老师和助教老师不能重复");
                    }
                }
                //老师集合name
                List<string> Teachers = new List<string>();
                Teachers.AddRange(input.MainTeacher); 
                input.AssistantTeacher?.AddRange(Teachers);

                //上课老师和助教老师未匹配薪资，课程时长不匹配
                int itemcount = 0;
                foreach (var item in Teachers)
                {

                    //查询薪资表中老师的薪资
                    var staffname = await _salarySettingRepository.FirstOrDefaultAsync(x => x.StaffName == item);

                    //查询薪资表中的课程时长
                    var hours = await _classHourFeeSettingRepository.GetQueryableAsync();
                    hours = hours.Where(x => x.SalarySettingId == staffname.Id);

                    //该老师没有课程时长，未分配薪资。
                    if (staffname.BasicSalaryType == SalaryType.底薪模式 && staffname.BasicSalary == 0)
                    {
                        return ApiResult<ClassScheduleDto>.Fail(ResultCode.Fail, $"{item}未分配薪资，无法选择。");
                    }
                    else if (staffname.BasicSalaryType == SalaryType.非底薪模式)
                    {
                        if (hours.FirstOrDefault().ClassHourDuration == 0)
                        {
                            return ApiResult<ClassScheduleDto>.Fail(ResultCode.Fail, $"{item}未分配薪资，无法选择。");
                        }
                    }
                    if (itemcount == 0)
                    {
                        //老师的课时时长不对。--主教第一个课时时长
                        //获取老师所有的课时
                        List<int> lists = hours.Select(x => x.ClassHourDuration).ToList();
                        //现排课老师的课时时长 
                        List<int> nows = input.ScheduleTimes.Select(x => (x.EndTime - x.StartTime).Minutes).ToList();
                        int tcount = 0;
                        foreach (var t in nows)
                        {
                            if (!lists.Contains(t))
                            {
                                return ApiResult<ClassScheduleDto>.Fail(ResultCode.Fail, $"{item}上课时间不匹配。该老师的上课时长有{lists}");
                            }
                        }
                    } 
                    itemcount++;
                    if (Teachers.Count == itemcount)
                    {
                        break;
                    }
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail(ResultCode.Fail, ex.Message);
            }
        }
    }
}
