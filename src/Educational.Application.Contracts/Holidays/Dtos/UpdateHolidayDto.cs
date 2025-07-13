using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Holidays.Dtos
{
	/// <summary>
	/// 修改节假日参数Dto
	/// </summary>
	public class UpdateHolidayDto
	{
		/// <summary>
		/// 节假日日期
		/// </summary>
		[Required(ErrorMessage = "节假日日期不能为空")]
		public DateTime Date { get; set; }
		/// <summary>
		/// 节假日名称
		/// </summary>
		[Required(ErrorMessage = "节假日名称不能为空")]
		[StringLength(50, ErrorMessage = "节假日名称不能超过50个字符")]
		public string HolidayName { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		[StringLength(500, ErrorMessage = "备注不能超过500个字符")]
		public string Remark { get; set; }
		/// <summary>
		/// 重复类型
		/// </summary>
		[Required(ErrorMessage = "重复类型不能为空")]
		public RepeatType RepeatType { get; set; }
		/// <summary>
		/// 重复日期
		/// </summary>
		public DateTime? RepeatUntil { get; set; }
	}
}
