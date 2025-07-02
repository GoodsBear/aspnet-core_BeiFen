using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.SalarySetting
{
    public interface ISalarySettingServices:IApplicationService
    {
        //修改
        Task<ApiResult<SalarySettingDto>> UpdateAsync(Guid id, SalarySettingDto input);
        //查询
        Task<ApiResult<ApiPaging<List<SalarySettingDto>>>> GetListAsync([FromQuery]SalarySeachDto search); 
    }
}
