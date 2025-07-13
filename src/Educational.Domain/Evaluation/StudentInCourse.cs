using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Educational.Evaluation
{
    /// <summary>
    /// 学生课程中间表
    /// </summary>
    public class StudentInCourse:Entity<Guid>
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }
}
