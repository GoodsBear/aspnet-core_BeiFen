using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Menu
{
    public class MenuTreeDto
    {
        public Guid value { get; set; }

        public string label { get; set; }

        public string path { get; set; }

        public string icon { get; set; }

        public List<MenuTreeDto> children { get; set; } = new List<MenuTreeDto>();
    }
}
