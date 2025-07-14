using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Holidays
{
	public class Holiday :FullAuditedAggregateRoot<Guid>
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
		/// 重复类型（YEARLY/MONTHLY/WEEKLY/NONE）
		/// </summary>
		[Required(ErrorMessage = "重复类型不能为空")]
		public RepeatType RepeatType { get; set; } = RepeatType.None;

		/// <summary>
		/// 重复截止日期
		/// </summary>
		public DateTime? RepeatUntil { get; set; }

		protected Holiday() { }

		
/// <summary>
/// 初始化 <see cref="Holiday"/> 类的新实例。
/// </summary>
/// <param name="date">假期的具体日期，包含时间信息。</param>
/// <param name="name">假期的名称，不能为空或空白字符串。</param>
/// <param name="remark">假期的备注信息，可为空。</param>
/// <param name="repeatType">假期的重复类型，默认为 <see cref="RepeatType.None"/>。</param>
/// <param name="repeatUntil">假期重复的截止日期，可为空。</param>
/// <exception cref="ArgumentException">当 <paramref name="name"/> 为 null 或空白字符串时抛出。</exception>
/// <returns>此构造函数不返回值，仅用于初始化对象。</returns>
public Holiday(
    DateTime date,
    string name,
    string remark = null,
    RepeatType repeatType = RepeatType.None,
    DateTime? repeatUntil = null)
{
    Date = date;
    HolidayName = Check.NotNullOrWhiteSpace(name, nameof(name));
    Remark = remark;
    RepeatType = repeatType;
    RepeatUntil = repeatUntil;
}
	}
	/// <summary>
	/// 重复类型
	/// </summary>
	public enum RepeatType
	{
		None,
		Yearly,
		Monthly,
		Weekly
	}
}
