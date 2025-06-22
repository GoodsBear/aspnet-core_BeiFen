using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educational.Enmu;

namespace Educational.Staffs
{
    public class ShowStaffDTO
    {
        /// <summary>
        /// 姓名（必填）
        /// </summary>
        public string StaffName { get; set; }

        /// <summary>
        /// 电话/登录账号（必填）
        /// </summary>
        public string StaffAccount { get; set; }
        /// <summary>
        /// 电话/登录账号（必填）
        /// </summary>
        public string StaffPassword { get; set; }
        /// <summary>
        /// 电话/登录账号（必填）
        /// </summary>
        public string StaffPhone { get; set; }

        /// <summary>
        /// 所属机构（必填）
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 性别（必填，例如 男/女/其他）
        /// </summary>
        public string StaffGender { get; set; }

        /// <summary>
        /// 职位（必填）
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 权限角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 人员类型
        /// </summary>
        public string StaffType { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// 当前状态（可用/禁用等）
        /// </summary>
        public StaffStatus Status { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 毕业学校
        /// </summary>
        public string GraduationSchool { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 照片（路径或URL）
        /// </summary>
        public string PhotoUrl { get; set; }
    }
}
