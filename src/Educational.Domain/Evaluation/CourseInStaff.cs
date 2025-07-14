using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Educational.Evaluation
{
    /// <summary>
    /// 课程员工中间表
    /// </summary>
    public class CourseInStaff : Entity<Guid>
    {
        public Guid CourseId { get; set; }
        public Guid StaffId { get; set; }
    }
}
