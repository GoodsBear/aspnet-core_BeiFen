using Educational.Enmu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Educational.Organization
{
    public class CreateUpdateOrganizationDto 
    { 
        public Guid Id { get; set; }
       
        public string Name { get; set; }

         
        public Guid LevelId { get; set; }

        
        public Guid PartentedId { get; set; }

       
        public string? ShortName { get; set; }

         
        public string? ContactPerson { get; set; }

       
        public string? Phone { get; set; }

       
        public string? Fax { get; set; }

     
        public string? Email { get; set; }

       
        public int SortOrder { get; set; } = 1;

       
        public SwitchEnum IsActive { get; set; } = SwitchEnum.启用;

        
        public string? Description { get; set; }
    }
}