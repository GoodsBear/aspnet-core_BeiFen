using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.SalarySetting
{
    [AutoMap(typeof(ClassHourFeeSetting))]
    public class ClassHourFeeSettingDto
    {
        public int ClassHourDuration { get; set; }
        public decimal ClassHourFee { get; set; }
        public decimal AssistantFee { get; set; }
    }
}
