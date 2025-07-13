using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.Evaluation
{
    public interface IEvaluationService:IApplicationService
    {
        Task<ApiResult> StudentLogin(StudentLoginDTO studentLoginDTO);
        Task<ApiResult<List<ShowEvaluationDTO>>> ShowEvaluationAsync();
        Task<ApiResult<List<ShowEvaluationDTO>>> AddEvaluationAsync(AddEvaluationDTO addEvaluationDTO);
    }
}
