using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.ClassSchedule
{
    /// <summary>
    /// 冲突表--分页
    /// </summary>
    public class ConflictModel : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 排课表主键
        /// </summary> 
        [Required(ErrorMessage = "排课表主键不能为空")]

        public Guid ClassScheduleId { get; set; } 
        /// <summary>
        /// 班级名称
        /// </summary>
        [Required(ErrorMessage = "班级名称不能为空")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "班级名称长度需在2-50个字符之间")]
        public string ClassName { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        [Required(ErrorMessage = "课程名称不能为空")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "课程名称长度需在2-100个字符之间")]
        public string CourseName { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        [Required(ErrorMessage = "上课日期不能为空")]
        [DataType(DataType.Date, ErrorMessage = "上课日期格式不正确")]
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 上课时间段（显示格式）
        /// </summary>
        [Required(ErrorMessage = "上课时间段不能为空")]
        [StringLength(20, ErrorMessage = "上课时间段长度不能超过20个字符")]
        public string ClassTime { get; set; }

        /// <summary>
        /// 开始时间（精确到分钟）
        /// </summary>
        [Required(ErrorMessage = "开始时间不能为空")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间（精确到分钟）
        /// </summary>
        [Required(ErrorMessage = "结束时间不能为空")]
        [CustomValidation(typeof(ConflictModel), "ValidateEndTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 上课教师
        /// </summary>
        [Required(ErrorMessage = "上课教师不能为空")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "教师姓名长度需在2-50个字符之间")]
        public string TeacherName { get; set; }
        /// <summary>
        /// 自定义验证：结束时间必须在开始时间之后
        /// </summary>
        //public static ValidationResult ValidateEndTime(TimeSpan endTime, ValidationContext context)
        //{
        //    var model = context.ObjectInstance as ConflictModel;
        //    if (model == null)
        //        return ValidationResult.Success;

        //    return endTime > model.StartTime
        //        ? ValidationResult.Success
        //        : new ValidationResult("结束时间必须在开始时间之后", new[] { nameof(EndTime) });
        //}
    }


    //    /// <summary>
    //    /// 冲突类型（0:时间冲突, 1:教室冲突, 2:教师冲突）
    //    /// </summary>
    //    [Range(0, 2, ErrorMessage = "冲突类型值必须在0-2之间")]
    //    public int ConflictType { get; set; }

    //    /// <summary>
    //    /// 冲突描述
    //    /// </summary>
    //    [StringLength(500, ErrorMessage = "冲突描述不能超过500个字符")]
    //    public string ConflictDescription { get; set; }
    //}

}
