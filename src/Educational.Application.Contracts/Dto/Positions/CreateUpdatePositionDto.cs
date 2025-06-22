using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.Positions
{
    public class CreateUpdatePositionDto
    {
        /// <summary>Add commentMore actions
        /// 职位名称
        /// </summary>Add commentMore actions
        [Required(ErrorMessage = "职位名称不能为空")]
        [StringLength(128)]
        public string PositionName { get; set; } = string.Empty;
    }
}
