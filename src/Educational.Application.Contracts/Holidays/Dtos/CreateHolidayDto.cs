using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Holidays.Dtos
{
	/// <summary>
	/// 添加节假日dto
	/// </summary>
	public class CreateHolidayDto
	{
		[Required(ErrorMessage = "节假日日期不能为空")]
		public DateTime Date { get; set; }

		[Required(ErrorMessage = "节假日名称不能为空")]
		[StringLength(50, ErrorMessage = "节假日名称不能超过50个字符")]
		public string HolidayName { get; set; }

		[StringLength(500, ErrorMessage = "备注不能超过500个字符")]
		public string Remark { get; set; }

		[Required(ErrorMessage = "重复类型不能为空")]
		public RepeatType RepeatType { get; set; } = RepeatType.None;

		public DateTime? RepeatUntil { get; set; }
	}
}
