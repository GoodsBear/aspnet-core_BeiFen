using Educational.Materials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.MaterialRecordsDtos
{
	/// <summary>
	/// 修改MaterialRecords的参数Dto
	/// </summary>
	public class CreateUpdateMaterialRecordsDto
	{
		/// <summary>
		/// 物料ID
		/// </summary>
		public Guid MaterialId { get; set; }

		/// <summary>
		/// 变动数量
		/// </summary>
		public int ChangeSum { get; set; }

		/// <summary>
		/// 申请员工ID
		/// </summary>
		public Guid StaffId { get; set; }

		/// <summary>
		/// 涉及学生ID（可选）
		/// </summary>
		public Guid? StudentId { get; set; }

		/// <summary>
		/// 变动类型
		/// </summary>
		public ChangeEnum ChangeType { get; set; }

		/// <summary>
		/// 原因
		/// </summary>
		public string Reason { get; set; }
        /// <summary>
        /// 变动时间
        /// </summary>
        public DateTime ChangeDate { get; set; }
	}
}
