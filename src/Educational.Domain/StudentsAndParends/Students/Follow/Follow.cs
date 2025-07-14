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
        /// 跟进方式途径枚举
        /// </summary>
        public GetInTouchEnum GetInTouchEnum { get; set; }
        /// <summary>
        /// 跟进阶段枚举
        /// </summary>
        public FollowStageEnums FollowStageEnum { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordDate { get; set;}
        /// <summary>
        /// 联系时间
        /// </summary>
        public DateTime TouchTIme { get; set; }
        /// <summary>
        /// 下次跟进时间
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
