using Educational.Enmu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Staffs
{
    /// <summary>
    /// 员工信息表
    /// 继承 FullAuditedAggregateRoot<Guid>，自动拥有审计字段
    /// </summary>
    public class StaffInfo : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 姓名（必填）
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Comment("姓名")]
        public string StaffName { get; set; }

        /// <summary>
        /// 电话/登录账号（必填）
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Comment("登录账号")]
        public string StaffAccount { get; set; }
        /// <summary>
        /// 电话/登录账号（必填）
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Comment("登录密码")]
        public string StaffPassword { get; set; }
        /// <summary>
        /// 电话/登录账号（必填）
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Comment("电话")]
        public string StaffPhone { get; set; }

        /// <summary>
        /// 所属机构（必填）
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Comment("所属机构")]
        public string Organization { get; set; }

        /// <summary>
        /// 性别（必填，例如 男/女/其他）
        /// </summary>
        [Required]
        [MaxLength(10)]
        [Comment("性别")]
        public string StaffGender { get; set; }

        /// <summary>
        /// 职位（必填）
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Comment("职位")]
        public string Position { get; set; }

        /// <summary>
        /// 权限角色
        /// </summary>
        [MaxLength(100)]
        [Comment("权限角色")]
        public string Role { get; set; }

        /// <summary>
        /// 人员类型
        /// </summary>
        [MaxLength(50)]
        [Comment("人员类型")]
        public string StaffType { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        [Comment("入职日期")]
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// 当前状态（可用/禁用等）
        /// </summary>
        [MaxLength(20)]
        [Comment("状态")]
        public StaffStatus Status { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [MaxLength(50)]
        [Comment("学历")]
        public string Education { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Comment("生日")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 毕业学校
        /// </summary>
        [MaxLength(100)]
        [Comment("毕业学校")]
        public string GraduationSchool { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [MaxLength(500)]
        [Comment("简介")]
        public string Introduction { get; set; }
        /// <summary>
        /// 照片（路径或URL）
        /// </summary>
        [MaxLength(200)]
        [Comment("照片")]
        public string PhotoUrl { get; set; }
    }
}
