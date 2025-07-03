using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.SalarySetting
{ 
    public class SalarySettingDto : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Required]
        public Guid StaffId { get; set; }

        // <summary>
        ///基本工资--模式
        /// </summary>
        public SalaryType BasicSalary { get; set; } = SalaryType.非底薪模式;

        /// <summary>
        /// 达标课时数
        /// </summary>
        public int? QualifiedClassHours { get; set; }

        /// <summary>
        /// 课时费设置列表
        /// </summary>
        public string? ClassHourFeeSettings { get; set; }

        /// <summary>
        /// 组织机构 
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}
