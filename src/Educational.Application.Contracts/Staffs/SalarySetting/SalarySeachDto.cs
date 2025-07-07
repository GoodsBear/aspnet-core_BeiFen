using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Staffs.SalarySetting
{
    public class SalarySeachDto:Seach
    {
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid? OrganizationId {  get; set; }
    }
}
