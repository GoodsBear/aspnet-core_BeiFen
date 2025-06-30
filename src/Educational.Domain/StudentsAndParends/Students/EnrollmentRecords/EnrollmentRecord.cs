using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.StudentsAndParends.Students.EnrollmentRecords
{
    /// <summary>
    /// 报名记录表
    /// </summary>
    public class EnrollmentRecord:FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// 报名类型枚举
        /// </summary>
        public enum RecordType : int
        {
            新签=1,
            续签=2
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ValidTime { get; set; }
        /// <summary>
        /// 购买课时数
        /// </summary>
        public int BugLessons { get; set; } 
        /// <summary>
        /// 课程金额
        /// </summary>
        public float LessonPrice { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public float FactGetPrice { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public float DisCountPrice  { get; set; }
        /// <summary>
        /// 收款经收人
        /// </summary>
        public Guid StaffId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Desc { get; set; }
    }
}
