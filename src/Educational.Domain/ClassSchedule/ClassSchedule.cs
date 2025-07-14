using Educational.Classgrade;
using Educational.Courses;
using Educational.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.ClassSchedule
{
    /// <summary>
    /// 排课表
    /// </summary>
    public class ClassSchedule : FullAuditedAggregateRoot<Guid>
    {
        // 关联分校 (必填)组织机构表
        [Required]
        public Guid OrganizationId { get; set; }
        public string? OrganizationName { get; set; }

        // 关联班级 (必填)
        [Required]
        public Guid ClassId { get; set; }
        public string? ClassName { get; set; }
        // 关联课程 (必填)
        [Required]
        public Guid CourseId { get; set; }
        public string? CourseName { get; set; }
        //主课老师
        [Required]
        public List<string> MainTeacher { get; set; }
        // 关联助教老师 (可选)  
        public List<string>? AssistantTeacher { get; set; } 
        // 时间范围 (必填)上课日期
        [Required]
        public DateTime StartDate { get; set; }
        [Required] 
        public DateTime EndDate { get; set; }

        // 排课规则 
        [Range(1, int.MaxValue, ErrorMessage = "消课基数必须大于0")]
        public int ConsumptionBase { get; set; } = 1;   // 消课基数 (默认值1)
        [Range(0, int.MaxValue, ErrorMessage = "人数不能为负数")]
        public int? MaxAttendees { get; set; } = null;       // 人数限制 (null/0=无限制) 
        public int? MaxSchedules { get; set; } = null;         // 最大排课次数 (null=无限制)
        public bool SkipHolidays { get; set; }       // 跳过节假日开关
        public bool IsTimetableGenerated { get; set; } = false;       // 是否生成课表--生成之后则不允许修改
        public bool HasSchedulingConflict { get; set; } = false;      // 是否有冲突
        public Guid?  ConflictId { get; set; }      // 冲突表外键 ConflictModel  
        public Guid?  ScheduleTimeId { get; set; }  //上课时间表 ScheduleTime 废了，先添加排课表，获取排课表主键再去添加上课时间表。永远为空把
        public int GeneratedSessionCount { get; set; } = 0;  // 生成课次
    }
}

