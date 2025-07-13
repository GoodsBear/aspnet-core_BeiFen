using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Holidays.Dtos
{
	/// <summary>
	/// 搜索节假日条件参数dto
	/// </summary>
	public class SearchHolidayDto
	{
		/// <summary>
		/// 节假日名称
		/// </summary>
		public string? HolidayName { get; set; }
		/// <summary>
		/// 年份
		/// </summary>
		public int Year { get; set; }
	}
}
