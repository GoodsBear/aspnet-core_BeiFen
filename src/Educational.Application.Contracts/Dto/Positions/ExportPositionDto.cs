using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.Positions
{
    public class ExportPositionDto
    {
        [DisplayName("职位名称")]
        public string PositionName { get; set; }
    }
}
