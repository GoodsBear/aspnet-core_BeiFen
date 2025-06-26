using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Students
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
		/// 手机号
		/// </summary>
		[Required(ErrorMessage = "手机号不能为空")]
		[MaxLength(11)]
		public string Phone { get; set; }
		/// <summary>
		/// 关联学生
		/// </summary>
		[MaxLength(500)]
		[Required(ErrorMessage = "关联学生不能为空")]
		public string Studentlist { get; set; }
		/// <summary>
		/// 微信昵称
		/// </summary>
		[Required(ErrorMessage = "微信昵称不能为空")]
		[MaxLength(20)]
		public string NickName { get; set; }
		/// <summary>
		/// 登录次数
		/// </summary>
		[Required(ErrorMessage = "登录次数不能为空")]
		[MaxLength(11)]
		public int LoginCount { get; set; } = 0;
		/// <summary>
		/// 上次登录时间
		/// </summary>
		public DateTime? LastLoginTime { get; set; }
		/// <summary>
		/// 上次登录IP
		/// </summary>
		[Required(ErrorMessage = "上次登录IP")]
		public string LastLoginIp { get; set; }
		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime RegisterTime { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
        public bool Status { get; set; }
	}
}
