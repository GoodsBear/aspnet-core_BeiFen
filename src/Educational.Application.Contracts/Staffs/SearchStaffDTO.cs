using Educational.Enmu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Staffs
{
    public class SearchStaffDTO:Seach
    {
        public string? StaffName { get; set; }
        public StaffStatus? Status { get; set; }
    }
}
