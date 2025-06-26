using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Educational.Dto.Grades
{
    public class GradeDto:AuditedEntityDto<Guid>
    {
        /// <summary>
		/// 年级名称
		/// </summary>
		public string?  GradeName { get; set; }
        /// <summary>
        /// 入学年份
        /// </summary>
        public DateTime? EnrollYear { get; set; }
        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
