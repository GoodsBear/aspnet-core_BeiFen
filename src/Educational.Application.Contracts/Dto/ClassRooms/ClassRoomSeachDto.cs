using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.ClassRooms
{
    public class ClassRoomSeachDto:Seach
    {
        /// <summary>
        /// 教室名称
        /// </summary>
        public string? ClassRoomName { get; set; }
    }
}
