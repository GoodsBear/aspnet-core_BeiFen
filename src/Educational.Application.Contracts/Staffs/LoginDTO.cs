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
        /// 登录账号（必填）
        /// </summary>
        public string StaffAccount { get; set; }
        /// <summary>
        /// 登录密码（必填）
        /// </summary>
        public string StaffPassword { get; set; }
        /// <summary>
        /// 获取验证码值（必填）
        /// </summary>
        public string CaptchaKey { get; set; }
        /// <summary>
        /// 验证码（必填）
        /// </summary>
        public string CaptchaCode { get; set; }

    }
}
