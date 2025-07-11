using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Staffs.SalarySetting
{
    public class HourDto
    { 
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
