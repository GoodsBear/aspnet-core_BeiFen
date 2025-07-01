using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Educational.SalarySetting.xin
{
    // 在Application项目中的SalarySettingDto.cs
    [AutoMapFrom(typeof(SalarySettingModel))] // 自动映射
    public class SalarySettingDto : EntityDto<Guid>
    {
        public decimal? BasicSalary { get; set; }
        public int? QualifiedClassHours { get; set; }
        public string Organization { get; set; }

        public List<ClassHourFeeSettingDto> ClassHourFeeSettings { get; set; }
    }
    [AutoMapFrom(typeof(ClassHourFeeSetting))]
    public class ClassHourFeeSettingDto : EntityDto<Guid>
    {
        public int ClassHourDuration { get; set; }
        public decimal ClassHourFee { get; set; }
        public decimal AssistantFee { get; set; }
    }
}
