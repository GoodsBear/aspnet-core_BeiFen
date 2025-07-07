using Abp.Domain.Entities.Auditing;
using Educational.Enmu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Organization
{ 
    public class OrganizationTreepageDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 机构名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public Guid LevelId { get; set; }

        /// <summary>
        /// 上级主键
        /// </summary>
        public Guid PartentedId { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string? ShortName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string? ContactPerson { get; set; }
        /// <summary>
        /// 电话
        /// </summary> 
        public string? Phone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string? Fax { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder { get; set; } = 1;

        /// <summary>
        /// 状态
        /// </summary>
        public SwitchEnum IsActive { get; set; } = SwitchEnum.启用;

        /// <summary>
        /// 说明
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 下级部门集合
        /// </summary>
        public IList Children { get; set; }
    }
}
