using NPOI.OpenXmlFormats.Dml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.ClassSchedule.Update
{
    public class UpdateClassScheduleDto
    {   
        // 关联分校 (必填)
        [Required]
        public Guid CampusId { get; set; }

        // 关联班级 (必填)
        [Required]
        public Guid ClassId { get; set; }
        // 关联课程 (必填)
        [Required]
        public Guid CourseId { get; set; }

        [Required]
        public List<string> MainTeacher { get; set; }
        // 关联助教老师 (可选)  
        public List<string>? AssistantTeacher { get; set; }
        // 时间范围 (必填)
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        // 排课规则
        [Range(1, int.MaxValue, ErrorMessage = "消课基数必须大于0")]
        public int ConsumptionBase { get; set; } = 1;   // 消课基数 (默认值1)
        [Range(0, int.MaxValue, ErrorMessage = "人数不能为负数")]
        public int? MaxAttendees { get; set; }          // 人数限制 (null/0=无限制)

        public int? MaxSchedules { get; set; }          // 最大排课次数 (null=无限制)

        public bool SkipHolidays { get; set; }       // 跳过节假日开关
        public bool IsTimetableGenerated { get; set; } = false;      // 是否生成课表--生成之后则不允许修改
        public bool HasSchedulingConflict { get; set; } = false;     // 是否有冲突 
        public int GeneratedSessionCount { get; set; } = 0;  // 生成课次
        // 子集合：具体上课时间
        public List<UpdateScheduleTime>? ScheduleTimes { get; set; } = new List<UpdateScheduleTime>();

    }
}
