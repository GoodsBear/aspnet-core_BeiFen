using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.Staffs.SalarySetting
{
    public interface ISalarySettingSearvies: IApplicationService
    {
        //修改
        Task<ApiResult> UpdateAsync(Guid id, SalarySettingDto input);
        //  Task<ApiResult<SalarySettingModel>> UpdateAsync(Guid id, SalarySettingDto input);
        //查询
        Task<ApiResult<ApiPaging<List<SalarySettingDto>>>> GetListAsync([FromQuery] SalarySeachDto search);
    }
}
