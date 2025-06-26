using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Dto.ClassRooms
{
    public class ClassRoomDto : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 教室名称
        /// </summary>
        public string ClassRoomName { get; set; }
        /// <summary>
        /// 分校
        /// </summary>
        public Guid OrganizationModelId { get; set; }
        /// <summary>
        /// 地点
        /// </summary>
        public string ClassRoomAddress { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public decimal ClassRoomArea { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string ClassRoomDescription { get; set; }
    }
}
