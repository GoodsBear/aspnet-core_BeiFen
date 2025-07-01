using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Guardian
{
    public class Guardian : FullAuditedAggregateRoot<Guid>
    {
        // <summary>
        /// 监护人姓名
        /// </summary>
        [Required(ErrorMessage = "监护人姓名不能为空。")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "监护人姓名长度应在2到50个字符之间。")]
        public string GuardianName { get; set; }

        /// <summary>
        /// 监护人电话\账号
        /// </summary>
        [Required(ErrorMessage = "监护人电话/账号不能为空。")]
        [Phone(ErrorMessage = "监护人电话格式不正确。")] // 或使用 Regex
                                              // [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "监护人电话格式不正确。")]
        public string GuardianPhone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空。")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度应在6到20个字符之间。")]
        public string GuardianPwd { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        [StringLength(50, ErrorMessage = "微信昵称长度不能超过50个字符。")]
        public string VChatName { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "登录次数必须为非负数。")]
        public int LoginNums { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Range(0, 1, ErrorMessage = "状态值必须为0或1。")] // 假设状态只有0和1
        public int Status { get; set; }
    }
}
