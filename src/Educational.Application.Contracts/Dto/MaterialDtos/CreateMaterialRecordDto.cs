using Educational.Materials;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Educational.Dto.MaterialDtos
{
    public class CreateMaterialRecordDto : EntityDto<Guid>
    {
        [Required(ErrorMessage = "必须指定物料")]
        public Guid MaterialId { get; set; }

        [Required(ErrorMessage = "变动数量不能为空")]
        [Range(1, 10000, ErrorMessage = "变动数量需在1-10000之间")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "必须指定操作员工")]
        public Guid StaffId { get; set; }

        public Guid? StudentId { get; set; }

        [Required(ErrorMessage = "必须填写变动原因")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "原因需在5-500个字符之间")]
        public string Reason { get; set; }
    }
} 