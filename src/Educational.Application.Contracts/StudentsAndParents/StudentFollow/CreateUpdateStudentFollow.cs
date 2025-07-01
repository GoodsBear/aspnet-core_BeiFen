using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StudentsAndParents.StudentFollow
{
    /// <summary>
    /// 学生跟随中间表
    /// </summary>
    public class CreateUpdateStudentFollow
    {
        public Guid StudentId { get; set; }

        public Guid FollowId { get; set; }

    }
}
