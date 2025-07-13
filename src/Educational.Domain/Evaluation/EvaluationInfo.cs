using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Evaluation
{
    /// <summary>
    /// 学评教
    /// </summary>
    public class EvaluationInfo : AuditedEntity<Guid>
    {
        /// <summary>
        /// 学员
        /// </summary>
        public Guid StudentId { get; set; }
        /// <summary>
        /// 老师
        /// </summary>
        public Guid TeacherId { get; set; }
        /// <summary>
        /// 课次标题
        /// </summary>
        public Guid CourseId { get; set; }
        /// <summary>
        /// 综合评分
        /// </summary>
        [Range(1, 5)]
        public int Comprehensive { get; set; }
        /// <summary>
        /// 课堂气氛
        /// </summary>
        [Range(1, 5)]
        public int Atmosphere { get; set; }
        /// <summary>
        /// 授课态度
        /// </summary>
        [Range(1, 5)]
        public int TeachingAttitude { get; set; }
        /// <summary>
        /// 教学效果
        /// </summary>
        [Range(1, 5)]
        public int teachingResult { get; set; }
        /// <summary>
        /// 评价内容
        /// </summary>
        public string Content { get; set; }=string.Empty;
    }
}
