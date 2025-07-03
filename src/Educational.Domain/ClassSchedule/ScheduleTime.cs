using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.ClassSchedule
{
    /// <summary>
    /// 上课时间表
    /// </summary>
    public class ScheduleTime : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 排课表主键
        /// </summary> 
        [Required(ErrorMessage = "排课表主键不能为空")]

        public Guid ClassScheduleId { get; set; }
        [Required]
        public DayOfWeek DayOfWeek { get; set; }    //(0-6;/日到6)// 星期（必填）
        [Required]
        //TimeSpan（存储HH:mm格式）
        public DateTime StartTime { get; set; }       // 开始时间（必填）

        [Required]
        public DateTime EndTime { get; set; }         // 结束时间（必填）

        public Guid? ClassroomId { get; set; }        // 教室（可选，对应"请选择"）
        public string ClassroomName { get; set; }        // 教室（可选，对应"请选择"）
    }
}
