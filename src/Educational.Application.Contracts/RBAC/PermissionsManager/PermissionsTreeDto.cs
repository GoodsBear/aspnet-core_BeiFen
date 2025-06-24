using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.RBAC.PermissionsManager
{
    public class PermissionsTreeDto
    {
        public Guid value { get; set; }

        public string label { get; set; }

        public List<PermissionsTreeDto>? children { get; set; }
    }
}
