using Educational.Dto.ClassRooms;
using Educational.Dto.Clbums;
using Educational.Dto.Grades;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.Clbums
{
    public interface IClassInfoAppService:IApplicationService
    {
        Task<ApiResult<ClassInfoDto>> CreateClass(CreateUpdateClassDto createUpdateClassDto);
        Task<ApiResult<ClassInfoDto>> UpdateClass(Guid id, CreateUpdateClassDto createUpdateClassDto);
        Task<ApiResult<ApiPaging<List<ClassInfoDto>>>> GetClassList([FromQuery] ClassSearchDto searchDto);
        Task<ApiResult> BatchDelete(List<Guid> ids);
        Task<ApiResult<List<ClassSelectDto>>> GetClassAsync();
        Task<ApiResult> BatchUpdateClassStatus(List<Guid> ids);
        //Task<ApiResult> UpdateClassStatus(Guid id, int Status);
    }
}
