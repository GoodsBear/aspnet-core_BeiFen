using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.StudentsAndParends.Students.Store
{
    /// <summary>
    /// 学生积分记录中间表
    /// </summary>
    public class StudentStoreRelation:FullAuditedAggregateRoot<Guid>
    {
        public Guid StudentId { get; set; }

        public Guid StoreId { get; set; }
    }
}
