using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Classgrade
{
	/// <summary>
	/// 年级
	/// </summary>
	public class Grade: FullAuditedAggregateRoot<Guid>
	{
		/// <summary>
		/// 年级名称
		/// </summary>
		public Guid GradeName { get; set; }
		/// <summary>
		/// 入学年份
		/// </summary>
        public DateTime? EnrollYear { get; set; }
		/// <summary>
		/// 排序值
		/// </summary>
        public int Sort { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
        public string Remark { get; set; }
	}
}
