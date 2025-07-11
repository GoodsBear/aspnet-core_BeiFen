using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.HomeWork
{
    /// <summary>
    /// 作业信息
    /// </summary>
    public class HomeWorkInfo:AuditedEntity<Guid>
    {
        /// <summary>
        /// 作业标题
        /// </summary>
        public string WorkTitle { get; set; }=string.Empty;
        /// <summary>
        /// 班级
        /// </summary>
        public Guid ClassId { get; set; }
        /// <summary>
        /// 提交数
        /// </summary>
        public int CommitNum { get; set; }
        /// <summary>
        /// 发起者
        /// </summary>
        public Guid Issuer { get; set; }
    }
}
