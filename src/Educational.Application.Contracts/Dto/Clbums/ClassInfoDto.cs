using Educational.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Dto.Clbums
{
    public class ClassInfoDto:AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 分校
        /// </summary>
        public Guid CampusId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 年级
        /// </summary>
        public Guid GradeId { get; set; }
        public string GradeName { get; set; }
        /// <summary>
        /// 班主任
        /// </summary>
        public Guid ClassTeacherId { get; set; }
        public string StaffName { get; set; }
        /// <summary>
        /// 预招人数
        /// </summary>
        public int PreNum { get; set; }
        /// <summary>
        /// 预排课次数
        /// </summary>
        public int PreCourseNum { get; set; }
        /// <summary>
        /// 默认课程
        /// </summary>
        public Guid DefaultCourseId { get; set; }
        public string CourseName { get; set; }
        /// <summary>
        /// 默认教室
        /// </summary>
        public Guid DefaultClassroomId { get; set; }
        public string ClassRoomName { get; set; }
        /// <summary>
        /// 计划开课日期
        /// </summary>
        public DateTime? PlanOpenDate { get; set; }
        /// <summary>
        /// 计划结业日期
        /// </summary>
        public DateTime? PlanCloseDate { get; set; }
        /// <summary>
        /// 班级群二维码
        /// </summary>
        public string ClassQrCode { get; set; }
        /// <summary>
        /// 排课备注
        /// </summary>
        public string CourseRemark { get; set; }
        /// <summary>
        /// 班级状态
        /// </summary>
        public LessonStateEnum ClassStatus { get; set; }
        public string ClassStatusName { get; set; }
    }
}
