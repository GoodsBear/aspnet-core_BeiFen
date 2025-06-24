using Educational.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Classgrade
{
	/// <summary>
	/// 班级
	/// </summary>
	public class ClassInfo:FullAuditedAggregateRoot<Guid>
	{
		/// <summary>
		/// 班级名称
		/// </summary>
        public string ClassName { get; set;}
		/// <summary>
		/// 分校
		/// </summary>
        public Guid CampusId { get; set;}
		/// <summary>
		/// 年级
		/// </summary>
        public Guid GradeId { get; set;}
		/// <summary>
		/// 班主任
		/// </summary>
        public Guid ClassTeacherId { get; set;}
		/// <summary>
		/// 预招人数
		/// </summary>
        public int PreNum { get; set;}
		/// <summary>
		/// 预排课次数
		/// </summary>
        public int PreCourseNum { get; set;}
		/// <summary>
		/// 默认课程
		/// </summary>
        public Guid DefaultCourseId { get; set;}
		/// <summary>
		/// 默认教室
		/// </summary>
        public Guid DefaultClassroomId { get; set;}
		/// <summary>
		/// 计划开课日期
		/// </summary>
        public DateTime? PlanOpenDate { get; set;}
		/// <summary>
		/// 计划结业日期
		/// </summary>
        public DateTime? PlanCloseDate { get; set;}
		/// <summary>
		/// 班级群二维码
		/// </summary>
        public string ClassQrCode { get; set;}
		/// <summary>
		/// 排课备注
		/// </summary>
        public string CourseRemark { get; set;}
		/// <summary>
		/// 班级状态
		/// </summary>
		public LessonStateEnum ClassStatus { get; set; } = LessonStateEnum.未开课;
	}
}
