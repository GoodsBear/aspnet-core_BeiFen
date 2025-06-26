using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		[Required(ErrorMessage = "年级名称不能为空")]
        [StringLength(50, ErrorMessage = "年级名称不能超过50个字符")]
		public string GradeName { get; set; }
		/// <summary>
		/// 入学年份
		/// </summary>
        public DateTime? EnrollYear { get; set; }
		/// <summary>
		/// 排序值
		/// </summary>
		[Range(0, 10000, ErrorMessage = "排序值不能小于0或大于10000")]
        [Required(ErrorMessage = "排序值不能为空")]
		public int Sort { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
        public string Remark { get; set; }
	}
}
