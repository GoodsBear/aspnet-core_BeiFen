using Educational.StudentsAndParends.Students.EnrollmentRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Educational.StudentsAndParents.EnrollmentRecords
{
    public class EnrollmentRecordDto:FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public Guid CourseId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 科目id
        /// </summary>
        public Guid SubjectId { get; set; }
        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 学生id
        /// </summary>
        public Guid StudentId { get; set; }
        /// <summary>
        /// 学生名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 报名类型枚举
        /// </summary>
        public RecordTypes RecordType { get; set; }
        public string RecordTypeName { get; set; }
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
        public decimal LessonPrice { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal FactGetPrice { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DisCountPrice { get; set; }
        /// <summary>
        /// 收款经收人
        /// </summary>
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Desc { get; set; }
        // <summary>
        /// 报名日期
        /// </summary>
        public DateTime EnrollmentDate { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public Examine ExamineStatus { get; set; } = Examine.审核中;
        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal? RefundMoney { get; set; }
        /// <summary>
        /// 退费次数
        /// </summary>
        public int? RefundNumber { get; set; }
        /// <summary>
        /// 退费说明
        /// </summary>
        public string? RefundDesc { get; set; }

    }
}
