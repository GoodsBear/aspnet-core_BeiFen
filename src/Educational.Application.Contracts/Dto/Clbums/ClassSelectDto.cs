using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.Clbums
{
    public class ClassSelectDto
    {
        /// <summary>
        /// 班级id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }
    }
}
