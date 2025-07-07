using JetBrains.Annotations;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.StudentsAndParends.Parents
{
	/// <summary>
	/// 家长
	/// </summary>
	public class Parent:FullAuditedAggregateRoot<Guid>
	{
		/// <summary>
		/// 家长姓名
		/// </summary>
		[Required(ErrorMessage = "家长姓名不能为空")]
        [MaxLength(20)]
		public string PardentName { get; set; }
		/// <summary>
		/// 手机号/账号
		/// </summary>
		[Required(ErrorMessage = "手机号不能为空")]
		[MaxLength(11)]
		public string Phone { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string ParentPwd { get; set; }
		/// <summary>
		/// 关联学生
		/// </summary>
		[MaxLength(10000)]
		public List<Guid>? Studentlist { get; set; }
		/// <summary>
		/// 微信昵称
		/// </summary>
		[Required(ErrorMessage = "微信昵称不能为空")]
		[MaxLength(20)]
		public string? NickName { get; set; }
		/// <summary>
		/// 登录次数
		/// </summary>
		[Required(ErrorMessage = "登录次数不能为空")]
		[MaxLength(11)]
		public int LoginCount { get; set; } = 0;
		/// <summary>
		/// 状态
		/// </summary>
        public bool Status { get; set; }
	}
}
