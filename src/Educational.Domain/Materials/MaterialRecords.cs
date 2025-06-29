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
	/// 物料出入库记录表
	/// </summary>
	public class MaterialRecords: FullAuditedAggregateRoot<Guid>
	{
		/// <summary>
		/// 物料ID
		/// </summary>
		[Required(ErrorMessage = "必须指定物料")]
		[RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
			ErrorMessage = "物料ID格式不正确")]
		public Guid MaterialId { get; set; }

		/// <summary>
		/// 变动数量
		/// </summary>
		[Range(-10000, 10000, ErrorMessage = "变动数量需在-10000到10000之间")]
		[NotZero(ErrorMessage = "变动数量不能为0")] // 需要自定义验证器
		public int ChangeSum { get; set; }

		/// <summary>
		/// 申请员工ID
		/// </summary>
		[Required(ErrorMessage = "必须指定操作员工")]
		[RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
			ErrorMessage = "员工ID格式不正确")]
		public Guid StaffId { get; set; }

		/// <summary>
		/// 涉及学生ID（可选）
		/// </summary>
		[RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
			ErrorMessage = "学生ID格式不正确")]
		public Guid? StudentId { get; set; }

		/// <summary>
		/// 变动类型
		/// </summary>
		[Required(ErrorMessage = "必须选择变动类型")]
		[EnumDataType(typeof(ChangeEnum), ErrorMessage = "无效的变动类型")]
		public ChangeEnum ChangeType { get; set; }

		/// <summary>
		/// 原因
		/// </summary>
		[Required(ErrorMessage = "必须填写变动原因")]
		[StringLength(500, MinimumLength = 5, ErrorMessage = "原因需在5-500个字符之间")]
		public string Reason { get; set; }
	}
	public class NotZeroAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			return value switch
			{
				int intVal => intVal != 0,
				long longVal => longVal != 0,
				decimal decimalVal => decimalVal != 0,
				_ => true
			};
		}
	}
}
