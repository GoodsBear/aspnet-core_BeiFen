using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StafRoles
{
    public class StaffRoleDto 
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid StaffId { get; set; }
        /// <summary>
        /// 角色Id集合Add
        /// </summary>
        public List<Guid> RoleIds { get; set; }
    }
}
