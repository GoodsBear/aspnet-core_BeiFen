using Educational.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Staffs
{
    /// <summary>
    /// 薪资表实体类
    /// </summary>
    public class SalarySettingModel : FullAuditedAggregateRoot<Guid>
    { 
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Required]
        public Guid StaffId { get; set; } 
        public string? StaffName { get; set; }
        // <summary>
        ///基本工资--模式
        /// </summary>
        public SalaryType BasicSalaryType { get; set; } 

        // <summary>
        ///底薪--模式
        /// </summary>
        public decimal? BasicSalary { get; set; }
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
