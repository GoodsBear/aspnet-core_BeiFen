using System;
using System.Collections.Generic;
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
		//家长姓名
        public string PardentName { get; set; }
		//手机号
        public string Phone { get; set; }
		//关联学生
		public string Studentlist { get; set; }
		//微信昵称
        public string NickName { get; set; }
		//登录次数
		public int LoginCount { get; set; } = 0;
		//上次登录时间
        public DateTime? LastLoginTime { get; set; }
        //上次登录IP
        public string LastLoginIp { get; set; }
		//注册时间
        public DateTime RegisterTime { get; set; }
		//状态
        public bool Status { get; set; }
	}
}
