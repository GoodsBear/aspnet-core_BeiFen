using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.StudentsAndParends.Students.Follow
{
    /// <summary>
    /// 跟进记录表
    /// </summary>
    public class Follow : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 联系途径枚举
        /// </summary>
        public enum GetInTouchEnum : int
        {
            面谈 = 1,
            电话联系 = 2,
            在线沟通 = 3,
            其他 = 4,
        }
        /// <summary>
        /// 跟进阶段枚举
        /// </summary>
        public enum FollowStageEnum : int
        {
            丢失阶段 = 1,
            目标客户阶段 = 2,
            潜在客户阶段 = 3,
            意向阶段 = 4,
            认可阶段 = 5,
            签约阶段 = 6,
            售后阶段 = 7,
        }


        /// <summary>
        /// 联系时间
        /// </summary>
        public DateTime TouchTIme { get; set; }
        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime? NextTouchTime { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string? TouchWay { get; set; }
        /// <summary>
        /// 跟进记录
        /// </summary>
        public string FollowDesc { get; set; }
    }
}
