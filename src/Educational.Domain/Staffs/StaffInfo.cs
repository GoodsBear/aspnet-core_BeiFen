using Educational.Enmu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required]
        [MaxLength(100)]
        [Comment("姓名")]
        [DisplayName("姓名")]
        public string StaffName { get; set; }

        [Required]
        [MaxLength(50)]
        [Comment("登录账号")]
        [DisplayName("登录账号")]
        public string StaffAccount { get; set; }

        [Required]
        [MaxLength(64)] // 存 SHA256 密码足够
        [Comment("登录密码")]
        [DisplayName("登录密码")]
        public string StaffPassword { get; set; }

        [MaxLength(20)]
        [Comment("电话")]
        [DisplayName("电话")]
        public string StaffPhone { get; set; }

        [MaxLength(100)]
        [Comment("所属机构")]
        [DisplayName("所属机构")]
        public string Organization { get; set; }

        [MaxLength(10)]
        [Comment("性别")]
        [DisplayName("性别")]
        public string StaffGender { get; set; }

        [MaxLength(50)]
        [Comment("职位")]
        [DisplayName("职位")]
        public string Position { get; set; }

        [MaxLength(50)]
        [Comment("权限角色")]
        [DisplayName("权限角色")]
        public string Role { get; set; }

        [MaxLength(50)]
        [Comment("人员类型")]
        [DisplayName("人员类型")]
        public string StaffType { get; set; }

        [Comment("入职日期")]
        [DisplayName("入职日期")]
        public DateTime? EntryDate { get; set; }

        [Comment("状态")]
        [DisplayName("状态")]
        public StaffStatus Status { get; set; }

        [MaxLength(50)]
        [Comment("学历")]
        [DisplayName("学历")]
        public string Education { get; set; }

        [Comment("生日")]
        [DisplayName("生日")]
        public DateTime? Birthday { get; set; }

        [MaxLength(50)]
        [Comment("毕业学校")]
        [DisplayName("毕业学校")]
        public string GraduationSchool { get; set; }

        [MaxLength(500)]
        [Comment("简介")]
        [DisplayName("简介")]
        public string Introduction { get; set; }

        [MaxLength(100)]
        [Comment("照片")]
        [DisplayName("照片")]
        public string PhotoUrl { get; set; }
    }
}
