using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StudentsAndParents.Students
{
    public class SearchStudentDto:Seach
    {
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string? StudentName { get; set; }
        /// <summary>
        /// 所属分校
        /// </summary>
        public Guid? OrgaizationId { get; set; }
        /// <summary>
        /// 所属年级
        /// </summary>
        public Guid? GradeId { get; set; }
    }
}
