using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.SpecialSubject
{ 
    /// <summary>
    /// 专题名称
    /// </summary>
    public class SpecialSubjectModel : AuditedAggregateRoot<Guid>
    {
        //// 构造函数
        //public SpecialSubjectModel()
        //{
        //    // 初始化默认值
        //    Details = string.Empty;
        //    AchievementDisplay = string.Empty;
        //}

        //public SpecialSubjectModel(
        //    string name,
        //    int categoryId,
        //    string teacher,
        //    string brief = "")
        //{
        //    Name = name;
        //    CategoryId = categoryId;
        //    Teacher = teacher;
        //    Brief = brief;
        //}
        [Required]
        [StringLength(128)] 
        public string Name { get; set; } // 专题名称（如"大祥"）

        [Required]
        public Guid CategoryId { get; set; } // 类别ID（如137）

        [StringLength(512)]
        public string LogoPath { get; set; } // Logo存储路径（图片上传后存储路径）

        [Required]
        [StringLength(64)]
        public string Teacher { get; set; } // 授课老师（如"王二鼻子"）

        [StringLength(500)]
        public string Brief { get; set; } // 简介（如"影楼一大糟"）

        [StringLength(500)]
        public string? Details { get; set; } // 详细介绍（富文本HTML内容）

        [StringLength(500)]
        public string? AchievementDisplay { get; set; } // 教学成果展示（富文本HTML内容）

 
    }
}
