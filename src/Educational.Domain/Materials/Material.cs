using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Materials
{
	/// <summary>
	/// 物料表
	/// </summary>
	public class Material: FullAuditedAggregateRoot<Guid>
	{
		/// <summary>
		/// 物料名称
		/// </summary>
		[Required(ErrorMessage = "物料名称不能为空")]
		[StringLength(100, MinimumLength = 2, ErrorMessage = "物料名称长度需在2-100个字符之间")]
		public string MaterialName { get; set; }

		/// <summary>
		/// 图片
		/// </summary>
		[Url(ErrorMessage = "图片地址格式不正确")]
		[MaxLength(500, ErrorMessage = "图片地址长度不能超过500字符")]
		public string MaterialImage { get; set; }

		/// <summary>
		/// 物料分类
		/// </summary>
		[EnumDataType(typeof(MaterialTypeEnum), ErrorMessage = "请选择有效的物料分类")]
		public MaterialTypeEnum MaterialTypeId { get; set; }

		/// <summary>
		/// 所属学校
		/// </summary>
		[Required(ErrorMessage = "必须指定所属学校")]
		[RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
			ErrorMessage = "学校ID必须是有效的GUID格式")]
		public Guid SchoolId { get; set; }

		/// <summary>
		/// 库存
		/// </summary>
		[Range(0, 100000, ErrorMessage = "库存数量必须在0-100000之间")]
		public int StockSum { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		[Required(ErrorMessage = "必须指定物料状态")]
		public bool Status { get; set; }
		/// <summary>
		/// 物料说明
		/// </summary>
		[StringLength(500, MinimumLength = 2, ErrorMessage = "描述长度需在2-500个字符之间")]
		[MaxLength(500, ErrorMessage = "描述长度不能超过500字符")]
		public string MterialDescription { get; set; }
	}
}
