using Educational.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Courses
{
	/// <summary>
	/// 课程
	/// </summary>
	public class Course:FullAuditedAggregateRoot<Guid>
	{
		/// <summary>
		/// 课程名称
		/// </summary>
		[Required(ErrorMessage ="课程名称不能为空")]
		[StringLength(50,MinimumLength =2,ErrorMessage ="课程名称长度在2-50个字符之间")]
        public string CourseName { get; set; }
		/// <summary>
		/// 适用学校
		/// </summary>
        public Guid? CampusId { get; set; }
		/// <summary>
		/// 科目
		/// </summary>
        public Guid? SubjectId { get; set; }
		/// <summary>
		/// 专题
		/// </summary>
        public Guid? TopicId { get; set; }
		/// <summary>
		/// 适用年级
		/// </summary>
		public Guid? GratorId { get; set; }
		/// <summary>
		/// 出售单位
		/// </summary>
		[Required(ErrorMessage ="出售单位不能为空")]
		public string SellUnit { get; set; }

		/// <summary>
		/// 课堂类型
		/// </summary>
		public CourseType CourseTypeId { get; set; }

		/// <summary>
		/// 总价
		/// </summary>
		[Range(0.01, 999999, ErrorMessage = "总价必须在0.01-999999之间")]
		public decimal TotalPrice { get; set; }
		/// <summary>
		/// 课时数
		/// </summary>
		[Range(1, 1000, ErrorMessage = "课时数必须在1-1000之间")]
		public int LessonNum { get; set; }

		/// <summary>
		/// 有效月数
		/// </summary>
		[Range(1, 36, ErrorMessage = "有效月数必须在1-36之间")]
		public int ValidMonthNum { get; set; }
		/// <summary>
		/// 是否预约
		/// </summary>
		public bool IsReserve { get; set; }
		/// <summary>
		/// 消课课酬
		/// </summary>
		[Range(0, 999999, ErrorMessage = "课酬金额必须在0-999999之间")]
		public decimal LessonCut { get; set; }
		/// <summary>
		/// 是否后付费
		/// </summary>
		public bool IsAfterPay { get; set; }
		/// <summary>
		/// 课酬计费模式
		/// </summary>
        public LessonCutModeEnum LessonCutMode { get; set; }
		/// <summary>
		/// 上课时长（分钟）
		/// </summary>
		[Range(30, 240, ErrorMessage = "上课时长必须在30-240分钟之间")]
		public int LessonDuration { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public bool Status { get; set; }


		//						***************************	
		//							在线购课报名设置
		//						***************************	

		/// <summary>
		/// 是否上架
		/// </summary>
		public bool IsOnlineSale { get; set; } = false;
		/// <summary>
		/// 课程封面图
		/// </summary>
		[Url(ErrorMessage = "封面图必须是有效的URL地址")]
		[MaxLength(500, ErrorMessage = "封面图URL长度不能超过500字符")]
		public string CoverImage { get; set; }
		/// <summary>
		/// 开启推荐
		/// </summary>
		public bool IsOpenRecommend { get; set; }
		/// <summary>
		/// 班级群二维码
		/// </summary>
		[Url(ErrorMessage = "二维码必须是有效的URL地址")]
		[MaxLength(500, ErrorMessage = "二维码URL长度不能超过500字符")]
		public string ClassQrCode { get; set; }
		/// <summary>
		/// 库存量
		/// </summary>
		[Range(0, 9999, ErrorMessage = "库存量必须在0-9999之间")]
		public int StockNum { get; set; }

		/// <summary>
		/// 停售日期
		/// </summary>
		[DataType(DataType.Date, ErrorMessage = "必须是有效的日期格式")]
		[FutureDate(ErrorMessage = "停售日期必须大于当前日期")] // 需要自定义验证器
		public DateTime? StopSaleDate { get; set; }

		/// <summary>
		/// 详情介绍图集（JSON数组格式）
		/// </summary>
		[JsonArray(ErrorMessage = "必须是合法的JSON数组格式")] // 需要自定义验证器
		public string DetailImageList { get; set; }

		/// <summary>
		/// 师资说明
		/// </summary>
		[MaxLength(1000, ErrorMessage = "师资说明不能超过1000字符")]
		public string TeacherRemark { get; set; }

		/// <summary>
		/// 服务说明
		/// </summary>
		[MaxLength(1000, ErrorMessage = "服务说明不能超过1000字符")]
		public string ServiceRemark { get; set; }
	}
	// 未来日期验证器
	public class FutureDateAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value is DateTime date)
			{
				return date > DateTime.Now;
			}
			return false;
		}
	}

	// JSON数组验证器
	public class JsonArrayAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value is not string json) return false;
			try
			{
				JsonDocument.Parse(json);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
