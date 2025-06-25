using Educational.Enmu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Staffs
{
    public class LoginReturnDTO:FullAuditedAggregateRoot<Guid>
    {
        [DisplayName("姓名")]
        public string StaffName { get; set; }

        [DisplayName("登录账号")]
        public string StaffAccount { get; set; }
        [DisplayName("电话")]
        public string StaffPhone { get; set; }
        [DisplayName("所属机构")]
        public string Organization { get; set; }

        [DisplayName("性别")]
        public string StaffGender { get; set; }
        [DisplayName("职位编号")]
        public Guid PositionId { get; set; }
        [DisplayName("职位")]
        public string Position { get; set; }
        [DisplayName("角色编号")]
        public Guid RoleId { get; set; }
        [DisplayName("权限角色")]
        public string Role { get; set; }

        [DisplayName("人员类型编号")]
        public Guid StaffTypeId { get; set; }
        [DisplayName("人员类型")]
        public string StaffType { get; set; }

        [DisplayName("入职日期")]
        public DateTime? EntryDate { get; set; }

        [DisplayName("状态")]
        public StaffStatus Status { get; set; }
        /// <summary>
        /// JWT登录Token
        /// </summary>
        public string Token { get; set; }
    }
}
