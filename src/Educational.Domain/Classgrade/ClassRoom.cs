using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Classgrade
{

    /// <summary>
    /// 教室表
    /// </summary>
    public class ClassRoom : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 教室名称
        /// </summary>
        [Required(ErrorMessage = "教室名称不能为空")]
        [StringLength(50, ErrorMessage = "教室名称不能超过50个字符")]
        public string ClassRoomName { get; set;}
        /// <summary>
        /// 分校
        /// </summary>
        [Required(ErrorMessage = "分校不能为空")]
        public Guid OrganizationModelId { get; set; }
        /// <summary>
        /// 地点
        /// </summary>
        [Required(ErrorMessage = "地点不能为空")]
        [StringLength(100, ErrorMessage = "地点不能超过100个字符")]
        public string ClassRoomAddress { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        [Required(ErrorMessage = "面积不能为空")]
        [Range(0, 10000, ErrorMessage = "面积不能小于0或大于10000")]
        public decimal ClassRoomArea { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500, ErrorMessage = "备注不能超过500个字符")]
        [Required(ErrorMessage = "备注不能为空")]
        public string ClassRoomDescription { get; set; }
    }
}
