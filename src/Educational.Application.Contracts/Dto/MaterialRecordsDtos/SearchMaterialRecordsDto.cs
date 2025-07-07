using Educational.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.MaterialRecordsDtos
{
	/// <summary>
	/// 查询条件
	/// </summary>
	public class SearchMaterialRecordsDto:Seach
	{
		/// <summary>
		/// 关联物料
		/// </summary>
		public Guid? MaterialId { get; set; }
		/// <summary>
		/// 关联学生
		/// </summary>
		public Guid? StudentId { get; set; }
		/// <summary>
		/// 申请员工
		/// </summary>
		public Guid? StaffId { get; set; }
		/// <summary>
		/// 变动类型
		/// </summary>
        public ChangeEnum? ChangeType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? endTime { get; set; }
    }
}
