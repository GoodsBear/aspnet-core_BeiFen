using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StudentsAndParents.Stores
{
    public class SearchDto:Seach
    {
       public DateTime? CreateTime { get; set; } 

        public DateTime? EndTime { get; set; }

    }
}
