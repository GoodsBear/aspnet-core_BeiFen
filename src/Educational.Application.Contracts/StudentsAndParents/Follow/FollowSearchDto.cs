using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StudentsAndParents.Follow
{
    public class FollowSearchDto:Seach
    {
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 跟进阶段
        /// </summary>
        public int? FollowStageEnum { get; set;}
        /// <summary>
        /// 跟进人
        /// </summary>
        public Guid? Consultant { get; set; }
    }
}
