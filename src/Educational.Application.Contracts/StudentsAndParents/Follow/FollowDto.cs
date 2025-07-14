using Educational.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace Educational.StudentsAndParents.StudentFollow
{

    public class FollowDto:FullAuditedEntityDto<Guid>
    {
        public Guid Id { get; set;}
        public Guid StudentId { get; set; }
        /// <summary>
        /// 顾问
        /// </summary>
        public Guid? Consultant { get; set;}
        public string ConsultantName { get; set; }
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 跟进id
        /// </summary>
        public Guid FollowId { get; set; }
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
        public DateTime RecordDate { get; set; }
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
