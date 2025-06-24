using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Organization
{
    /// <summary>
    /// 组织机构树状数据结构Dto
    /// </summary>
    public class OrganizationTreeDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 下级部门集合
        /// </summary>
        public List<OrganizationTreeDto> Chlidren { get; set; }
    }
}
