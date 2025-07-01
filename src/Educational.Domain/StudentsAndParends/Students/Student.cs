using Educational.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.StudentsAndParends.Students
{
	public class Student:FullAuditedAggregateRoot<Guid>
	{
		/// <summary>
		/// 学员姓名
		/// </summary>
		[MaxLength(20)]
        [Required(ErrorMessage = "学员姓名不能为空")]
		public string Name { get; set; }
		/// <summary>
		/// 手机号
		/// </summary>
		[MaxLength(11)]
        [Required(ErrorMessage = "手机号不能为空")]
		public string Phone { get; set; }
		/// <summary>
		/// 所属校区
		/// </summary>
		[Required(ErrorMessage = "所属校区不能为空")]
		public Guid CampusId { get; set; }
		/// <summary>
		/// 家长姓名
		/// </summary>
		[MaxLength(20)]
        [Required(ErrorMessage = "家长姓名不能为空")]
		public string ParentName { get; set; }
		/// <summary>
		/// 亲属关系
		/// </summary>
		public Relation Relation { get; set; }
		/// <summary>
		/// 学生性别
		/// </summary>
		[Required(ErrorMessage = "学生性别不能为空")]
        [MaxLength(10)]
		public SexEnum Sex { get; set; }
		/// <summary>
		/// 入学时间
		/// </summary>
		[MaxLength(10)]
        [Required(ErrorMessage = "入学时间不能为空")]
		public DateTime? EnrollTime { get; set; }
		/// <summary>
		/// 年级
		/// </summary>
		[Required(ErrorMessage = "年级不能为空")]
		public Guid GradeId { get; set; }
		/// <summary>
		/// 出生年月
		/// </summary>
		public DateTime? Birthday { get; set; }
		/// <summary>
		/// 身份证号
		/// </summary>
		[MaxLength(18)]
        [Required(ErrorMessage = "身份证号不能为空")]
		public string IdCard { get; set; }
		/// <summary>
		/// 来源
		/// </summary>
		[MaxLength(50)]
        [Required(ErrorMessage = "来源不能为空")]
		public string Source { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(500)]
        [Required(ErrorMessage = "备注不能为空")]
		public string Remark { get; set; }
        /// <summary>
        /// 学员类型
        /// </summary>
		public StudentEnum StudentType { get; set; }
        /// <summary>
        /// 顾问
        /// </summary>
        public Guid Consultant { get; set; }
		/// <summary>
		/// 课时数
		/// </summary>
		public int LessonNums { get; set; }
		/// </summary>
		/// <summary>
		/// 年龄
		/// </summary>
		public int? Age { get; set; }
	}
}
