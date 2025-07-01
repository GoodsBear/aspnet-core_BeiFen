using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.StudentsAndParends.Students.Store
{
    /// <summary>
    /// 学生积分变动记录表
    /// </summary>
    public class Store:FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 变动原因
        /// </summary>
        public string ChangeCause { get; set; }
        /// <summary>
        /// 变动积分
        /// </summary>
        public int ChangeStores { get; set; }
        /// <summary>
        /// 剩余积分
        /// </summary>
        public int ExcessStores { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Desc { get; set; }
    }
}
