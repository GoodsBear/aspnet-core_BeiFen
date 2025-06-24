using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Materials
{
	/// <summary>
	/// 物料表
	/// </summary>
	public class Material: FullAuditedAggregateRoot<Guid>
	{
		//物料名称
        public string MaterialName { get; set; }
		//图片
        public string MaterialImage { get; set; }
		//物料分类
        public MaterialTypeEnum MaterialTypeId { get; set; }
		//所属学校
        public Guid SchoolId { get; set; }
		//库存
        public int StockSum { get; set; }
		//状态
		public bool Status { get; set; }
	}
}
