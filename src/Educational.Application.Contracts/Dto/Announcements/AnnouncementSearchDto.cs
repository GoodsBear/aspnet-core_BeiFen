using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.Announcements
{
    public class AnnouncementSearchDto:Seach
    {
        /// <summary>
        /// 公告标题
        /// </summary>
        public string? Title { get; set; }
    }
}
