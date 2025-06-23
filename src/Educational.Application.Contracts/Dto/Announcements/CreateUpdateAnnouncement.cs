using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.Announcements
{
    public class CreateUpdateAnnouncement
    {
        /// <summary>
        /// 公告标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 公告内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
