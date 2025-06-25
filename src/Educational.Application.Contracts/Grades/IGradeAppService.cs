using Educational.Dto.Grades;
using Educational.Dto.Positions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.Grades
{
    public interface IGradeAppService : IApplicationService
    {
        Task<ApiResult<GradeDto>> CreatePosition(CreateUpdateGradeDto createUpdateGradeDto);
        Task<ApiResult<GradeDto>> UpdatePosition(Guid id, CreateUpdateGradeDto createUpdateGradeDto);
        Task<ApiResult<ApiPaging<List<GradeDto>>>> GetPositionList([FromQuery] GradeSearchDto searchDto);
        Task<ApiResult> BatchDelete(List<Guid> ids);
    }
}
