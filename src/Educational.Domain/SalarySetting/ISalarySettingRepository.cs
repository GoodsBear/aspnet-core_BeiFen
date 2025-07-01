using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Educational.SalarySetting  
{ 
    // 在Domain项目中的ISalarySettingRepository.cs
    public interface ISalarySettingRepository : IRepository<SalarySettingModel, Guid>
    {
        // 按组织架构分页查询薪资设置（包含课时费明细）
        Task<PagedResultDto<SalarySettingModel>> GetSalarySettingsByOrganizationAsync(
            string organization,
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
        );
    }
}
