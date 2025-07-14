using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.ClassSchedule.Update
{
    public class UpdateScheduleTime
    {
        [Required]
        public DayOfWeek DayOfWeek { get; set; }    //(0-6;/日到6)// 星期（必填）
        [Required]
        //TimeSpan（存储HH:mm格式）
        public DateTime StartTime { get; set; }       // 开始时间（必填）

        [Required]
        public DateTime EndTime { get; set; }         // 结束时间（必填）

        public Guid ClassroomId { get; set; }        // 教室（可选，对应"请选择"） 
    }
}
