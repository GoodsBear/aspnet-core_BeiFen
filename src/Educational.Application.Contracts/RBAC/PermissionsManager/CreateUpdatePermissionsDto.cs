using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.RBAC.PermissionsManager
{
    public class CreateUpdatePermissionsDto
    {
        public string PermissionName { get; set; } //权限名称

        public string PermissionsDesc { get; set; } //权限描述

        public Guid ParentId { get; set; } //父级ID，默认为0表示顶级权限
    }
}
