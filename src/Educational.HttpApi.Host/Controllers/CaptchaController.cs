using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Educational.Controllers
{
    [ApiExplorerSettings(GroupName = "成员")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptcha captcha;

        public CaptchaController(ICaptcha captcha)
        {
            this.captcha = captcha;
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Captcha(string id)
        {
            var info = captcha.Generate(id);
            // 有多处验证码且过期时间不一样，可传第二个参数覆盖默认配置。
            //var info = _captcha.Generate(id,120);
            var stream = new MemoryStream(info.Bytes);
            return File(stream, "image/gif");
        }
    }
}
