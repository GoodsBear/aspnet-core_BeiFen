using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Menu
{
    public class PermissionInfo
    {
        public Guid Id { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; } // 权限标识符
        public string PermissionType { get; set; } // "menu", "button", "api" 等
    }
}
