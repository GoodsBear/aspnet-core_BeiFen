using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.SalarySetting
{
    /// <summary>
    /// 课时费设置
    /// </summary>
    public class ClassHourFeeSetting : FullAuditedAggregateRoot<Guid>
    {
        //薪资表
        public Guid SalarySettingId { get; set; }   // 外键
        /// <summary>
        /// 课时时长（分钟）
        /// </summary>
        public int ClassHourDuration { get; set; }

        /// <summary>
        /// 课时费（元）
        /// </summary>
        public decimal ClassHourFee { get; set; }

        /// <summary>
        /// 助教费（元）
        /// </summary>
        public decimal AssistantFee { get; set; }
    }
}
