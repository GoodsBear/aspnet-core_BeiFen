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
    public class LoginReturnDTO
    {
        /// <summary>
        /// JWT登录Token
        /// </summary>
        public string Token { get; set; }
    }
}
