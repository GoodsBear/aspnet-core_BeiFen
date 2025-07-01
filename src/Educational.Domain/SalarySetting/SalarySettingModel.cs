using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.SalarySetting
{
    /// <summary>
    /// 薪资表实体类
    /// </summary>
    public class SalarySettingModel : FullAuditedAggregateRoot<Guid>
    {
        public string StaffinfoName { get; set; }   /// <summary>
                                                    /// </summary>
        public decimal? BasicSalary { get; set; }

        /// <summary>
        /// 达标课时数
        /// </summary>
        public int? QualifiedClassHours { get; set; }

        /// <summary>
        /// 课时费设置列表
        /// </summary>
        public List<ClassHourFeeSetting>? ClassHourFeeSettings { get; set; }

        /// <summary>
        /// 组织机构
        /// 用于标识该薪资设置所属的组织架构，比如学校的某个校区、部门等
        /// </summary>
        public string Organization { get; set; }
    }
}
