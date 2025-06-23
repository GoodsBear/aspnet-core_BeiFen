using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Educational.Controllers
{
    [Route("api/upload")]
    public class UploadController : AbpController
    {
        private readonly IWebHostEnvironment env;

        public UploadController(IWebHostEnvironment env)
        {
            this.env = env;
        }
        /// <summary>
        /// 上传图片到 wwwroot/upload/images 文件夹
        /// </summary>
        /// <param name="file">上传的图片文件</param>
        /// <returns>返回图片完整访问 URL</returns>
        [HttpPost("image")]
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            // 校验文件是否为空
            if (file == null || file.Length == 0)
            {
                throw new UserFriendlyException("上传文件不能为空！");
            }

            // 校验文件大小（最大 2MB）
            const long maxSizeInBytes = 2 * 1024 * 1024; // 2MB
            if (file.Length > maxSizeInBytes)
            {
                throw new UserFriendlyException("图片大小不能超过 2MB！");
            }

            // 校验文件扩展名（限制图片格式）
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (Array.IndexOf(allowedExtensions, ext) < 0)
            {
                throw new UserFriendlyException("仅支持图片格式：jpg, jpeg, png, gif, bmp");
            }

            // 确定保存路径： wwwroot/upload/images
            var uploadFolder = Path.Combine(env.WebRootPath, "upload", "images");
            if (!Directory.Exists(uploadFolder))
            {
                // 如果目录不存在则创建
                Directory.CreateDirectory(uploadFolder);
            }

            // 生成唯一文件名
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadFolder, fileName);

            // 保存文件到目标路径
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 构建完整 URL，便于前端直接访问
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fileUrl = $"{baseUrl}/upload/images/{fileName}";

            return fileUrl;
        }
    }
}
