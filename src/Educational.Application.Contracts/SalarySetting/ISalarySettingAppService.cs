using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.SalarySetting
{
    public interface ISalarySettingAppService : IApplicationService
    { 
        //修改薪资表+添加课费表
        Task<ApiResult> UpdateAsync(Guid id,SalarySettingDto input);
        //查询（根据组织机构名称查询） 
        Task<ApiResult<ApiPaging<List<SalaryTimeDto>>>> GetAsync([FromQuery] SalarySearch search); 
    }
}
