using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Educational.ClassSchedule
{
    /// <summary>
    /// 上课时间表
    /// </summary>
    public class ScheduleTime : Entity<Guid>
    {
        [Required]
        public DayOfWeek DayOfWeek { get; set; }     // 星期（必填）

        [Required]
        //TimeSpan（存储HH:mm格式）
        public TimeSpan StartTime { get; set; }       // 开始时间（必填）

        [Required]
        public TimeSpan EndTime { get; set; }         // 结束时间（必填）

        public Guid? ClassroomId { get; set; }        // 教室（可选，对应"请选择"）
        public string ClassroomName { get; set; }        // 教室（可选，对应"请选择"）

        // 关联主表
        public Guid ClassScheduleId { get; set; }
    }
}
