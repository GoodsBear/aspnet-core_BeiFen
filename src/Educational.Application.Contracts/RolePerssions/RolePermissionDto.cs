using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.RolePerssions
{
    public class RolePermissionDto
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 权限id
        /// </summary>
        public List<Guid> PermissionIds { get; set; }
    }
}
