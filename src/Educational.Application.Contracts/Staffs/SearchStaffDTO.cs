using Educational.Enmu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Staffs
{
    // 员工搜索条件
    public class SearchStaffDTO : Seach
    {
        public string? StaffName { get; set; }  // 按姓名搜索（模糊匹配）
        public StaffStatus? Status { get; set; }  // 按状态筛选
        public Guid? OrganizationId { get; set; }  // 按机构ID筛选
    }
}
