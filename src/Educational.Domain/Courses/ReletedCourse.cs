using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Courses
{
	/// <summary>
	/// 课程关联
	/// </summary>
	public class ReletedCourse:FullAuditedAggregateRoot<Guid>
	{
        public Guid Course1Id { get; set; }
		public Guid Course2Id { get; set; }
	}
}
