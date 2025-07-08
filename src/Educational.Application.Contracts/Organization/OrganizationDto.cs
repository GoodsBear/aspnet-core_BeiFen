using Educational.Enmu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Organization
{
    public class OrganizationDto : AuditedEntityDto<Guid>
    { 
        /// <summary>
        /// 机构名
        /// </summary>
        [Required(ErrorMessage = "机构名是必填项")]
        [StringLength(100, ErrorMessage = "机构名长度不能超过100字符")]
        [Display(Name = "机构名")]
        public string Name { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [Required(ErrorMessage = "级别是必填项")]
        [Display(Name = "级别")]
        public Guid LevelId { get; set; }

        /// <summary>
        /// 上级主键
        /// </summary>
        [Required(ErrorMessage = "级别是必填项")]
        [Display(Name = "级别")]
        public Guid PartentedId { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [StringLength(50, ErrorMessage = "简称长度不能超过50字符")]
        [Display(Name = "简称")]
        public string? ShortName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [StringLength(50, ErrorMessage = "联系人长度不能超过50字符")]
        [Display(Name = "联系人")]
        public string? ContactPerson { get; set; }
        /// <summary>
        /// 电话
        /// </summary> 
        [StringLength(20, ErrorMessage = "电话长度不能超过20字符")]
        [Phone(ErrorMessage = "请输入有效的电话号码")]
        [Display(Name = "电话")]
        public string? Phone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [StringLength(20, ErrorMessage = "传真长度不能超过20字符")]
        [Display(Name = "传真")]
        public string? Fax { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(100, ErrorMessage = "邮箱长度不能超过100字符")]
        [EmailAddress(ErrorMessage = "请输入有效的邮箱地址")]
        [Display(Name = "邮箱")]
        public string? Email { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "排序值必须大于0")]
        [Display(Name = "排序")]
        public int SortOrder { get; set; } = 1;

        /// <summary>
        /// 状态
        /// </summary>
        [Required]
        [Display(Name = "状态")]
        public SwitchEnum IsActive { get; set; } = 0;

        /// <summary>
        /// 说明
        /// </summary>
        [StringLength(500, ErrorMessage = "说明长度不能超过500字符")]
        [Display(Name = "说明")]
        public string? Description { get; set; }
    }
}
