using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.SalarySetting
{
    [AutoMap(typeof(SalarySettingModel))]
    public class SalarySettingDto : FullAuditedAggregateRoot<Guid>
    {
        
        [Required]
        public string StaffinfoName { get; set; }

        public bool IsBasicSalaryMode { get; set; }

        public decimal? BasicSalary { get; set; }

        public int? QualifiedClassHours { get; set; }

        [Required]
        public List<ClassHourFeeSettingDto>? ClassHourFeeSettings { get; set; } = new();

        public string Organization { get; set; }   // ABP中可能从当前用户获取，所以DTO不一定要传递，但需要设置
    }
}
