using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Educational.EntityFrameworkCore;
using Educational.Organization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Educational.SalarySetting
{
    [ApiExplorerSettings(GroupName ="薪资")]
    public class SalarySettingAppService :ApplicationService, ISalarySettingAppService
    {
        private readonly ISalarySettingRepository _salarySettingRepository;
        private readonly IRepository<SalarySettingModel, Guid> salarySettingRep;
        private readonly IRepository<ClassHourFeeSetting, Guid> classHourFeeSettingRep;
        private readonly IRepository<OrganizationModel, Guid> organizationRepository;
        ILogger<SalarySettingAppService> logger;
         
        public SalarySettingAppService(IRepository<SalarySettingModel, Guid> salarySettingRep, IRepository<OrganizationModel, Guid> organizationRepository, IRepository<ClassHourFeeSetting, Guid> classHourFeeSettingRep, ILogger<SalarySettingAppService> logger,
            ISalarySettingRepository _salarySettingRepository)
        {
            this._salarySettingRepository = _salarySettingRepository;
            this.salarySettingRep = salarySettingRep;
            this.organizationRepository = organizationRepository;
            this.classHourFeeSettingRep = classHourFeeSettingRep; 
            this.logger = logger;
        } 
        //添加在员工表添加中 
        //修改
        [UnitOfWork]
        public async Task<ApiResult> UpdateAsync(Guid id, SalarySettingDto input)
        {
            // 验证输入
            ValidateInput(input); 
            var entity = await salarySettingRep.GetAsync(id);
            if (entity.Id != id)
            {
                throw new AbpAuthorizationException("无权修改其他成员的薪资设置");
            } 
            // ABP的MapTo方法更新主实体
            ObjectMapper.Map(input, entity); 

            var result = ObjectMapper.Map<SalarySettingDto, SalarySettingModel>(input);
            //result.Id = id;
            var  sa = await salarySettingRep.UpdateAsync(result);
            if (sa == null)
            { 
                 return ApiResult.Fail(ResultCode.Fail, $"修改失败");
            } 


            // 处理子集合：先清空再添加（或者复杂一点比较更新）
            //entity.ClassHourFeeSettings.Clear();

            //foreach (var feeSettingDto in input.ClassHourFeeSettings)
            //{
            //    entity.ClassHourFeeSettings.Add(ObjectMapper.Map<ClassHourFeeSetting>(feeSettingDto));
            //} 
            //await _salarySettingRepository.UpdateAsync(entity);
            return ApiResult.Success(ResultCode.Ok); 
        }
       
        //public async Task<ApiResult<ApiPaging<List<SalaryTimeDto>>>> GetAsync([FromQuery] SalarySearch search)
        //{
        //    try
        //    {
        //        var dbContext = await GetDbContextAsync();
        //        var query = dbContext.Set<SalarySettingModel>()
        //            .Include(s => s.ClassHourFeeSettings) // 加载嵌套集合
        //            .Where(s => s.Organization == organization) // 按组织过滤
        //            .AsNoTracking(); // 无跟踪查询提升性能[4](@ref)
        //        var Hourlist = await classHourFeeSettingRep.GetQueryableAsync();
        //        var list = await salarySettingRep.GetQueryableAsync().Include(s => s.ClassHourFeeSettings);
        //        list = list.WhereIf(!string.IsNullOrEmpty(search.Organization), x => x.Organization.Contains(search.Organization));

        //        var linq = from a in list
        //                   join b in Hourlist
        //                    on a.Id equals b.SalarySettingId
        //                   select new SalaryTimeDto
        //                   {
        //                       Id = a.Id,
        //                       ClassHourFeeSettings = b.ToList(),
        //                   };
        //        var page = list.PageResult(search.PageIndex, search.PageSize);
        //        var result = new ApiPaging<List<SalaryTimeDto>>()
        //        {
        //            TotleCount = page.RowCount,
        //            TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / search.PageSize),
        //            Data = linq.ToList()
        //        };
        //        return ApiResult<ApiPaging<List<SalaryTimeDto>>>.Success(ResultCode.Ok, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("员工薪资查询异常" + ex.Message);
        //        return ApiResult<ApiPaging<List<SalaryTimeDto>>>.Fail(ResultCode.Fail, $"员工薪资查询异常: {ex.Message}");
        //    }

        //}

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="UserFriendlyException"></exception>
        private void ValidateInput(SalarySettingDto input)
        {

            if (input.IsBasicSalaryMode)
            {
                if (input.BasicSalary < 0)
                {
                    throw new UserFriendlyException("底薪不能为负数");
                }
                if (input.QualifiedClassHours < 0)
                {
                    throw new UserFriendlyException("达标课时数不能为负数");
                }
            }
            else
            {
                if (input.ClassHourFeeSettings == null || input.ClassHourFeeSettings.Count == 0)
                {
                    throw new UserFriendlyException("非底薪模式必须至少设置一项课时费");
                }

                // 检查课时时长是否有重复
                var durations = input.ClassHourFeeSettings.Select(s => s.ClassHourDuration).ToList();
                if (durations.Distinct().Count() != durations.Count)
                {
                    throw new UserFriendlyException("课时时长有重复");
                }

                // 验证每个课时设置项
                foreach (var setting in input.ClassHourFeeSettings)
                {
                    if (setting.ClassHourDuration <= 0)
                    {
                        throw new UserFriendlyException("课时时长必须大于0");
                    }
                    if (setting.ClassHourFee < 0)
                    {
                        throw new UserFriendlyException("课时费不能为负数");
                    }
                    if (setting.AssistantFee < 0)
                    {
                        throw new UserFriendlyException("助教费不能为负数");
                    }
                }
            }
        }

        public async Task<ApiResult<ApiPaging<List<SalaryTimeDto>>>> GetAsync([FromQuery] SalarySearch search)
        {
            throw new NotImplementedException();
        }
        //查询
        public async Task<PagedResultDto<SalarySettingDto>> GetlisstAsync(
       string organization,
       PagedAndSortedResultRequestDto input)
        {
            throw new NotImplementedException();
            //// 调用仓储获取领域模型
            //var settings = await _salarySettingRepository.GetSalarySettingsByOrganizationAsync(
            //    organization,
            //    input.SkipCount,
            //    input.MaxResultCount
            //);
            //// 映射到DTO（AutoMapper自动处理嵌套集合）
            //var a = settings.MapTo<PagedResultDto<SalarySettingDto>>(settings);
        }
    }
}
