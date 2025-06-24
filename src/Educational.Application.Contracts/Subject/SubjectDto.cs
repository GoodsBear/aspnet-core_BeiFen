using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Subject
{
    public class SubjectDto : FullAuditedAggregateRoot<Guid>
    { 

            /// <summary>
            /// 科目名称（如“数学”“英语”）
            /// </summary>
            [Required(ErrorMessage = "科目名称是必填项")]
            [MaxLength(20)]
            public string SubjectName { get; set; } = string.Empty;

            /// <summary>
            /// 排序权重，数值越大越靠前
            /// </summary>
            [Column("sort_weight")]
            public int SortWeight { get; set; } = 0;

            /// <summary>
            /// 科目详细说明
            /// </summary>
            [MaxLength(200)]
            public string? SubjectDescription { get; set; }
        }
    }
