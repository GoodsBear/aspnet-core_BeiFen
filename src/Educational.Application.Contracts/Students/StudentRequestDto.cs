using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Educational.Students
{
	/// <summary>
	/// 学生查询参数
	/// </summary>
	public class StudentRequestDto: Seach
	{
		/// <summary>
		/// 学生名称
		/// </summary>
		public string? StudentName { get; set; }
		/// <summary>
		/// 分校
		/// </summary>
        public Guid? CampusId { get; set; }
		/// <summary>
		/// 年级
		/// </summary>
        public Guid? GradeId { get; set; }
		/// <summary>
		/// 授课老师
		/// </summary>
        public Guid? ClassTeacherId { get; set; }
	}
}
