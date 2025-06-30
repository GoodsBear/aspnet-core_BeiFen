using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StafRoles
{
    public class RoleStaffDto
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 用户Id集合
        /// </summary>
        public List<Guid>  StaffIds { get; set; }
       
    }
}
