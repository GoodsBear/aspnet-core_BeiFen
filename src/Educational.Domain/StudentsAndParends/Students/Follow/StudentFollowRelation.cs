using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.StudentsAndParends.Students.Follow
{
    /// <summary>
    /// 学员跟进记录中间表
    /// </summary>
    public class StudentFollowRelation : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 学员Id
        /// </summary>
        public Guid StudentId { get; set; }
        /// <summary>
        /// 跟进记录Id
        /// </summary>
        public Guid FollowId { get; set; }
    }
}
