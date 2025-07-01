using Educational.Enums;
using System;
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
        public GetInTouchEnum GetInTouchEnum { get; set; }
        /// <summary>
        /// 跟进阶段枚举
        /// </summary>
        public FollowStageEnum FollowStageEnum { get; set; }


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
