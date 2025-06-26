using Educational.Materials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Dto.MaterialDtos
{
	public class MaterialDto: AuditedAggregateRoot<Guid>
	{
		/// <summary>
		/// 物料名称
		/// </summary>
		public string MaterialName { get; set; }

		/// <summary>
		/// 图片
		/// </summary>
		public string MaterialImage { get; set; }

		/// <summary>
		/// 物料分类
		/// </summary>
		public MaterialTypeEnum MaterialTypeId { get; set; }

		/// <summary>
		/// 所属学校
		/// </summary>
		public Guid SchoolId { get; set; }

		/// <summary>
		/// 库存
		/// </summary>
		public int StockSum { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		public bool Status { get; set; }
	}
}
