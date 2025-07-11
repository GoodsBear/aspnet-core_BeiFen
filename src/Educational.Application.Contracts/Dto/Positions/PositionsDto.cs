using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Educational.Dto.Positions
{
    public class PositionsDto: EntityDto<Guid>
    {
        /// <summary>Add commentMore actions
        /// 职位名称
        /// </summary>
        public string PositionName { get; set; }= string.Empty;
    }
}
