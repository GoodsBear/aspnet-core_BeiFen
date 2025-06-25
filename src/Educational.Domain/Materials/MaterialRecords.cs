using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Materials
{
	/// <summary>
	/// 物料出入库记录表
	/// </summary>
	public class MaterialRecords: FullAuditedAggregateRoot<Guid>
	{
		//物料
        public Guid MaterialId { get; set; }
		//变动数量
        public int ChangeSum { get; set; }
		//申请员工
        public Guid StaffId { get; set; }
		//涉及学生
        public Guid? StudentId { get; set; }
		//变动类型
        public ChangeEnum ChangeType { get; set; }
		//原因
        public string Reason { get; set; }
	}
}
