using Educational.SalarySetting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Educational.EntityFrameworkCore
{
    // 在EntityFrameworkCore项目中的SalarySettingRepository.cs
    public class SalarySettingRepository : EfCoreRepository<EducationalDbContext, SalarySettingModel, Guid>, ISalarySettingRepository
    {
         
        public SalarySettingRepository(IDbContextProvider<EducationalDbContext> dbContextProvider)
            : base(dbContextProvider) { }

        public async Task<PagedResultDto<SalarySettingModel>> GetSalarySettingsByOrganizationAsync(
            string organization,
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
        )
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.Set<SalarySettingModel>()
                .Include(s => s.ClassHourFeeSettings) // 加载嵌套集合
                .Where(s => s.Organization == organization) // 按组织过滤
                .AsNoTracking(); // 无跟踪查询提升性能[4](@ref)

            // 分页并执行查询
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(s => s.CreationTime)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);

            return new PagedResultDto<SalarySettingModel>(totalCount, items);
        }
         
    }
}
