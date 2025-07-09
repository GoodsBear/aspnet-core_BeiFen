using Educational.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Courses
{
	public class CreateCourseDto
	{
		public Guid Id { get; set; }
		/// <summary>
		/// 课程名称
		/// </summary>
		public string CourseName { get; set; }
		/// <summary>
		/// 适用学校
		/// </summary>
		public Guid CampusId { get; set; }
		/// <summary>
		/// 科目
		/// </summary>
		public Guid SubjectId { get; set; }
		/// <summary>
		/// 专题
		/// </summary>
		public Guid TopicId { get; set; }
		/// <summary>
		/// 适用年级
		/// </summary>
        public Guid GratorId { get; set; }
		///出售单位
        public string SellUnit { get; set; }

		/// <summary>
		/// 课堂类型
		/// </summary>
		public CourseType CourseTypeId { get; set; }
		/// <summary>
		/// 总价
		/// </summary>
		public decimal TotalPrice { get; set; }
		/// <summary>
		/// 课时数
		/// </summary>
		public int LessonNum { get; set; }
		/// <summary>
		/// 有效月数
		/// </summary>
		public int ValidMonthNum { get; set; }
		/// <summary>
		/// 是否预约
		/// </summary>
		public bool IsReserve { get; set; }
		/// <summary>
		/// 消课课酬
		/// </summary>
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
		/// 上课时长
		/// </summary>
		public int LessonDuration { get; set; }
		//状态
		public bool Status { get; set; }


		//						***************************	
		//							在线购课报名设置
		//						***************************	


		/// <summary>
		/// 是否上架
		/// </summary>
		public bool IsOnlineSale { get; set; }
		/// <summary>
		/// 课程封面图
		/// </summary>
		public string? CoverImage { get; set; }
		/// <summary>
		/// 开启推荐
		/// </summary>
		public bool IsOpenRecommend { get; set; }
		/// <summary>
		/// 班级群二维码
		/// </summary>
		public string? ClassQrCode { get; set; }
		/// <summary>
		/// 库存量
		/// </summary>
		public int StockNum { get; set; }
		/// <summary>
		/// 停售日期
		/// </summary>
		public DateTime? StopSaleDate { get; set; }
		/// <summary>
		/// 详情介绍图集
		/// </summary>
		public string? DetailImageList { get; set; }
		/// <summary>
		/// 师资说明
		/// </summary>
		public string TeacherRemark { get; set; }
		/// <summary>
		/// 服务说明
		/// </summary>
		public string ServiceRemark { get; set; }
	}
}
