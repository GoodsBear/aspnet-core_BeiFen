using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Educational.Dto.Announcements
{
    public class AnnouncementDto: AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 公告标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 公告内容
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
