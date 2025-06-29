using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Positions
{
    /// <summary>
    /// 职位表
    /// </summary>
    public class Position:FullAuditedAggregateRoot<Guid>
    {
        /// <summary>Add commentMore actions
        /// 职位名称
        /// </summary>
        [Required(ErrorMessage = "职位名称不能为空")]

        public string PositionName { get; set; } = string.Empty;
    }
}
