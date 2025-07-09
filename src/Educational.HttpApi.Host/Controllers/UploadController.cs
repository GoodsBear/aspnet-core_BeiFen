using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        /// 上传图片到 wwwroot/Uploads/images/yyyyMMdd 目录
        /// </summary>
        /// <param name="file">上传的图片文件</param>
        /// <returns>文件完整访问 URL</returns>
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

            // 校验文件扩展名
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (Array.IndexOf(allowedExtensions, ext) < 0)
            {
                throw new UserFriendlyException("仅支持图片格式：jpg, jpeg, png, gif, bmp，webp");
            }

            // 根据日期生成子文件夹（yyyyMMdd）
            var dateFolder = DateTime.Now.ToString("yyyyMMdd");
            var uploadFolder = Path.Combine(env.WebRootPath, "Uploads", "images", dateFolder);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // 生成唯一文件名
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadFolder, fileName);

            // 保存文件
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 构建完整访问 URL
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fileUrl = $"{baseUrl}/Uploads/images/{dateFolder}/{fileName}";

            return fileUrl;
        }

		/// <summary>
		/// 批量上传图片到 wwwroot/Uploads/images/yyyyMMdd 目录
		/// </summary>
		/// <param name="files">上传的图片文件集合</param>
		/// <returns>所有图片的完整访问URL列表</returns>
		[HttpPost("images")]
		public async Task<List<string>> UploadImagesAsync(List<IFormFile> files)
		{
			if (files == null || files.Count == 0)
				throw new UserFriendlyException("请选择要上传的图片！");

			var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
			const long maxSizeInBytes = 2 * 1024 * 1024; // 2MB
			var dateFolder = DateTime.Now.ToString("yyyyMMdd");
			var uploadFolder = Path.Combine(env.WebRootPath, "Uploads", "images", dateFolder);

			if (!Directory.Exists(uploadFolder))
				Directory.CreateDirectory(uploadFolder);

			var fileUrls = new List<string>();
			var request = HttpContext.Request;
			var baseUrl = $"{request.Scheme}://{request.Host}";

			foreach (var file in files)
			{
				try
				{
					if (file == null || file.Length == 0)
						continue; // 跳过空文件

					if (file.Length > maxSizeInBytes)
						throw new UserFriendlyException("图片大小不能超过2MB！");

					var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
					if (!allowedExtensions.Contains(ext))
						throw new UserFriendlyException("仅支持图片格式: jpg, jpeg, png, gif, bmp, webp");

					var fileName = $"{Guid.NewGuid()}{ext}";
					var filePath = Path.Combine(uploadFolder, fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}

					var fileUrl = $"{Request.Scheme}://{Request.Host}/Uploads/images/{dateFolder}/{fileName}";
					fileUrls.Add(fileUrl);
				}
				catch (Exception ex)
				{
					// 记录日志，继续处理下一个文件
					// logger.LogError(ex, $"文件上传失败: {file?.FileName}");
					// 也可以选择将错误信息返回给前端
					continue;
				}
			}

			if (fileUrls.Count == 0)
				throw new UserFriendlyException("没有有效的图片被上传！");

			return fileUrls;
		}
	}
}
