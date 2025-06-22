using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Staffs
{
    public class LoginDTO
    {
        /// <summary>
        /// 电话/登录账号（必填）
        /// </summary>
        public string StaffAccount { get; set; }
        /// <summary>
        /// 电话/登录账号（必填）
        /// </summary>
        public string StaffPassword { get; set; }
    }
}
