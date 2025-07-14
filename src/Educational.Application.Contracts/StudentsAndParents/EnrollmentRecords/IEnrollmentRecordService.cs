using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.StudentsAndParents.EnrollmentRecords
{
    public interface IEnrollmentRecordService : IApplicationService
    {
        Task<ApiResult<EnrollmentRecordDto>> AddEnrollmentAsync(CreateUpdateEnrollmentRecordDto createUpdateEnrollmentRecordDto);
        Task<ApiResult<List<EnrollmentRecordDto>>> BatchAddEnrollmentAsync(List<CreateUpdateEnrollmentRecordDto> dtos);
        Task<ApiResult<ApiPaging<List<EnrollmentRecordDto>>>> GetEnrollmentRecordList([FromQuery] EnrollmentSeachDto seachDto);
        
    }
}
