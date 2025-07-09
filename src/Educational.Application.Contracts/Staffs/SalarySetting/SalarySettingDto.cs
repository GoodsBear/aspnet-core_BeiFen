using Educational.Enums;
using Educational.SalarySetting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Staffs.SalarySetting
{ 
     public class SalarySettingDto 
    {
        public  Guid Id { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Required]
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        
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
        /// 课时费设置列表  "60:200.00:50.00,90:300.00:75.00,120:400.00:100.00"
        /// 60 200 50
        /// 90 300 75
        /// 120 400 100
        /// </summary>
        public List<UpdateSalaryDto>? ClassHourFeeSettings { get; set; }

        /// <summary>
        /// 组织机构 
        /// </summary>
        public Guid? OrganizationId { get; set; }
        /// <summary>
        /// 课时时长（分钟）
        /// </summary>
        public int ClassHourDuration { get; set; } = 0;
        /// <summary>
        /// 课时费（元）
        /// </summary>
        public decimal ClassHourFee { get; set; } = 0;

        /// <summary>
        /// 助教费（元）
        /// </summary>
        public decimal AssistantFee { get; set; } = 0;
    }
}
