using Educational.Materials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Dto.MaterialRecordsDtos
{
	/// <summary>
	/// 返回DTO
	/// </summary>
	public class MaterialRecordsDto:AuditedAggregateRoot<Guid>
	{
		/// <summary>
		/// 物料ID
		/// </summary>
		public Guid MaterialId { get; set; }
		/// <summary>
		/// 物料名称
		/// </summary>
        public string MaterialName { get; set; }

		/// <summary>
		/// 变动数量
		/// </summary>
		public int ChangeSum { get; set; }

		/// <summary>
		/// 申请员工ID
		/// </summary>
		public Guid StaffId { get; set; }
		/// <summary>
		/// 员工姓名
		/// </summary>
        public string StaffName { get; set; }

		/// <summary>
		/// 涉及学生ID（可选）
		/// </summary>
		public Guid? StudentId { get; set; }
		/// <summary>
		/// 学生姓名
		/// </summary>
		public string StudentName { get; set; }

		/// <summary>
		/// 变动类型
		/// </summary>
		public ChangeEnum ChangeType { get; set; }

		/// <summary>
		/// 原因
		/// </summary>
		public string Reason { get; set; }
	}
}
