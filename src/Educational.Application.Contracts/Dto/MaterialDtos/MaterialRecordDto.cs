using Educational.Materials;
using System;
using Volo.Abp.Application.Dtos;

namespace Educational.Dto.MaterialDtos
{
    public class MaterialRecordDto : AuditedEntityDto<Guid>
    {
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; }
        public int ChangeSum { get; set; }
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        public Guid? StudentId { get; set; }
        public string StudentName { get; set; }
        public ChangeEnum ChangeType { get; set; }
        public string ChangeTypeName { get; set; }
        public string Reason { get; set; }
    }
} 