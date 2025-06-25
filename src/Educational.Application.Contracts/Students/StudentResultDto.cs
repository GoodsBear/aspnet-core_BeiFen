using Educational.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Educational.Students
{
	public class StudentResultDto:FullAuditedEntityDto<Guid>
	{
		/// <summary>
		/// 学生Id
		/// </summary>
		public Guid Id { get; set; }	
		/// <summary>
		/// 学生姓名
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 手机号
		/// </summary>
		public string Phone { get; set; }
		/// <summary>
		/// 所属校区
		/// </summary>
		public Guid CampusId { get; set; }
		//校区名称
        public string CampusName { get; set; }
		/// <summary>
		/// 家长姓名
		/// </summary>
		public string ParentName { get; set; }
		/// <summary>
		/// 亲属关系
		/// </summary>
		public FamilyEnum Relation { get; set; }
		/// <summary>
		/// 学生性别
		/// </summary>
		public SexEnum Sex { get; set; }
		/// <summary>
		/// 入学时间
		/// </summary>
		public DateTime? EnrollTime { get; set; }
		/// <summary>
		/// 年级
		/// </summary>
		public Guid GradeId { get; set; }
		//年级名称
        public string GradeName { get; set; }
		/// <summary>
		/// 出生年月
		/// </summary>
		public DateTime Birthday { get; set; }
		/// <summary>
		/// 身份证号
		/// </summary>
		public string IdCard { get; set; }
		/// <summary>
		/// 来源
		/// </summary>
		public string Source { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; }
	}
}
