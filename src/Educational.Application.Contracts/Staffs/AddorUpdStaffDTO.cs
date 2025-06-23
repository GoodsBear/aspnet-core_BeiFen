using Educational.Enmu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Staffs
{
    public class AddorUpdStaffDTO
    {
        /// <summary>
        /// 姓名（必填）
        /// </summary>
        [Required]
        [Comment("姓名")]
        public string StaffName { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [Required]
        [Comment("登录账号")]
        public string StaffAccount { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required]
        [Comment("登录密码")]
        public string StaffPassword { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [Comment("电话")]
        public string StaffPhone { get; set; }

        /// <summary>
        /// 所属机构（必填）
        /// </summary>
        [Required]
        [Comment("所属机构")]
        public string Organization { get; set; }

        /// <summary>
        /// 性别（必填，例如 男/女/其他）
        /// </summary>
        [Required]
        [Comment("性别")]
        public string StaffGender { get; set; }

        /// <summary>
        /// 职位（必填）
        /// </summary>
        [Required]
        [Comment("职位")]
        public string Position { get; set; }

        /// <summary>
        /// 权限角色
        /// </summary>
        [Comment("权限角色")]
        public string Role { get; set; }

        /// <summary>
        /// 人员类型
        /// </summary>
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
        [Comment("状态")]
        public StaffStatus Status { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
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
        [Comment("毕业学校")]
        public string GraduationSchool { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Comment("简介")]
        public string Introduction { get; set; }
        /// <summary>
        /// 照片（路径或URL）
        /// </summary>
        [Comment("照片")]
        public string PhotoUrl { get; set; }
    }
}
