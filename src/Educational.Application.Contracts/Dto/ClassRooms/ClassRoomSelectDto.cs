using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.ClassRooms
{
    /// <summary>
    /// 教室下拉框
    /// </summary>
    public class ClassRoomSelectDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 教室名称
        /// </summary>
        public string ClassRoomName { get; set; }
    }
}
