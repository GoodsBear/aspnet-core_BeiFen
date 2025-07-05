using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StafRoles
{
    public class GetRoleIdToShowStaffDto
    {
        public Guid RoleId { get; set; }

        public List<StaffDto> children { get; set; } = new List<StaffDto>();
    }

    public class StaffDto
    {
        /// <summary>
        /// 成员Id
        /// </summary>
        public Guid StaffId { get; set; }
        /// <summary>
        /// 成员姓名
        /// </summary>
        public string StaffName { get; set; } = string.Empty;
        /// <summary>
        /// 成员电话
        /// </summary>
        public string StaffTel { get; set; } = string.Empty;
        /// <summary>
        /// 成员职位
        /// </summary>
        public string StaffJob { get; set; } = string.Empty; 
    }
}
