using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Educational.RBAC.PermissionsManager
{
    public class PermissionsDto:FullAuditedEntityDto<Guid>
    {
        public string PermissionName { get; set; } //权限名称

        public string PermissionsDesc { get; set; } //权限描述
    }
}
