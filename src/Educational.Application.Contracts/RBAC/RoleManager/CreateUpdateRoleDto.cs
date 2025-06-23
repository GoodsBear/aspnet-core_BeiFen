using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.RBAC.RoleManager
{
    public class CreateUpdateRoleDto
    {
        public string RoleName { get; set; }

        public string RoleEncode { get; set; }

        public string RoleDesc { get; set; }

        public int RoleStatus { get; set; } // 1:启用, 0:禁用
    }
}
