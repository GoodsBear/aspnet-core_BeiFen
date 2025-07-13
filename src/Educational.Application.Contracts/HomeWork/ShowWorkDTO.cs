using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Educational.HomeWork
{
    public class ShowWorkDTO:Entity<Guid>
    {
        /// <summary>
        /// 作业标题
        /// </summary>
        public string WorkTitle { get; set; } = string.Empty;
        /// <summary>
        /// 班级
        /// </summary>
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        /// <summary>
        /// 提交数
        /// </summary>
        public int CommitNum { get; set; }
        /// <summary>
        /// 发起者
        /// </summary>
        public Guid Issuer { get; set; }
        public string IssuerName { get; set; } = string.Empty;
    }
}
