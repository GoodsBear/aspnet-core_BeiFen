using Abp.Application.Services;
using AutoMapper.Internal.Mappers;
using Educational.Subject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Educational.Dto.Positions;
using Educational.Organization;
using Educational.Positions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
namespace Educational.SalarySetting
{
    public class SalarySettingServices : Volo.Abp.Application.Services.ApplicationService, ISalarySettingServices
    {
        IRepository<SalarySettingModel, Guid> _salarySettingRepository;
        IRepository<ClassHourFeeSetting, Guid> _classHourFeeSettingRepository;
        ILogger<SalarySettingServices> logger;

        public SalarySettingServices(IRepository<SalarySettingModel, Guid> salarySettingRepository, IRepository<ClassHourFeeSetting, Guid>  classHourFeeSettingRepository, ILogger<SalarySettingServices> logger)
        {
            _salarySettingRepository =  salarySettingRepository;
            _classHourFeeSettingRepository = classHourFeeSettingRepository;
            this.logger = logger;
        }
        //获取列表
        public async Task<ApiResult<ApiPaging<List<SalarySettingDto>>>> GetListAsync([FromQuery] SalarySeachDto search)
        {
            try
            {
                // 构建查询
                var model = await _salarySettingRepository.GetQueryableAsync();
                // 机构查询
                model = model.WhereIf(search.OrganizationId!=null, x => x.OrganizationId.Equals(search.OrganizationId));
                // 使用ABP自带分页方法 
                var page = model.PageResult(search.PageIndex, search.PageSize);
                // 映射 
                List<SalarySettingDto> dto= ObjectMapper.Map<List<SalarySettingModel>, List<SalarySettingDto>>(model.ToList());
                var result = new ApiPaging<List<SalarySettingDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / search.PageSize),
                    Data = dto
                };
                return ApiResult<ApiPaging<List<SalarySettingDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("显示异常" + ex);
                return ApiResult<ApiPaging<List<SalarySettingDto>>>.Fail(ResultCode.Fail, $"显示异常: {ex.Message}");
            } 
        }
        //修改

        public Task<ApiResult<SalarySettingDto>> UpdateAsync(Guid id, SalarySettingDto input)
        {
            throw new NotImplementedException();
        }
    }
}
