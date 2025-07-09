using AutoMapper;
using Educational.Enums;
using Educational.Organization;
using Educational.SalarySetting;
using Educational.Staffs.SalarySetting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NPOI.OpenXmlFormats.Wordprocessing;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Staffs
{
    [ApiExplorerSettings(GroupName = "薪资")]
    public class SalarySettingSearvices : ApplicationService, ISalarySettingSearvies
    {
        private readonly IRepository<SalarySettingModel, Guid> salarySettingRepository;
        private readonly IRepository<StaffInfo, Guid> staffInfoRepository;
        private readonly IRepository<ClassHourFeeSetting, Guid> classHourFeeSettingRepository;
        ILogger<StaffServices> logger;

        public SalarySettingSearvices(
            IRepository<SalarySettingModel, Guid> salarySettingRepository, ILogger<StaffServices> logger, IRepository<StaffInfo, Guid> staffInfoRepository, IRepository<ClassHourFeeSetting, Guid> classHourFeeSettingRepository = null)
        {
            this.salarySettingRepository = salarySettingRepository;
            this.logger = logger;
            this.staffInfoRepository = staffInfoRepository;
            this.classHourFeeSettingRepository = classHourFeeSettingRepository;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<ApiPaging<List<SalarySettingDto>>>> GetListAsync([FromQuery] SalarySeachDto search)
        {
            try
            {
                // 获取薪资数据源
                var staffinfo = await salarySettingRepository.GetQueryableAsync();
                //OrganizationId
                staffinfo=staffinfo.WhereIf(search.OrganizationId != null, x => x.OrganizationId.Equals(search.OrganizationId));
                //根据反填的员工id查找员工姓名// staffinfo.StaffName
                var staff = await staffInfoRepository.GetQueryableAsync();

                // 课时费表
                var hoursmoeny = await classHourFeeSettingRepository.GetQueryableAsync();

                // 按组织部门Id查询
                staffinfo = staffinfo.WhereIf(search.OrganizationId != null, x => x.OrganizationId.Equals(search.OrganizationId));


                //
                var linq = from salary in staffinfo
                           join sta in staff
                           on salary.StaffId equals sta.Id
                           join moeny in hoursmoeny
                           on salary.Id  equals moeny.SalarySettingId
                           select new  SalarySettingDto
                           {
                               Id = salary.Id,
                               StaffId = sta.Id,
                               StaffName = sta.StaffName,
                               BasicSalaryType = salary.BasicSalaryType,
                               BasicSalary = salary.BasicSalary,
                               QualifiedClassHours = salary.QualifiedClassHours,
                               OrganizationId = salary.OrganizationId,
                               ClassHourDuration= moeny.ClassHourDuration,
                               ClassHourFee = moeny.ClassHourFee,
                               AssistantFee = moeny.AssistantFee
                           };

                // 使用ABP自带分页方法 
                var page = staffinfo.PageResult(search.PageIndex, search.PageSize);
                // 映射 
                //var salaryDto = ObjectMapper.Map<List<SalarySettingModel>, List<SalarySettingDto>>(page.Queryable.ToList());
               

                // 封装分页数据
                var result = new ApiPaging<List<SalarySettingDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / search.PageSize),
                    Data = linq.ToList()
                };

                // 返回成功结果
                return ApiResult<ApiPaging<List<SalarySettingDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "薪资信息获取失败");
                return ApiResult<ApiPaging<List<SalarySettingDto>>>.Fail(ResultCode.Fail, $"薪资信息获取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult> UpdateAsync(Guid id, SalarySettingDto input)
        {
            if (input == null)
            {
                return ApiResult<SalarySettingModel>.Fail(ResultCode.Fail, "输入参数无效");
            }

            try
            {
                // 查询薪资设置
                var salarySetting = await salarySettingRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (salarySetting == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "薪资设置不存在");
                }

                // 查询关联的课时费设置
                var hoursSettings = await classHourFeeSettingRepository.GetQueryableAsync();
                hoursSettings = hoursSettings.Where(x => x.SalarySettingId == id);

                // 批量删除旧的课时费设置
                await classHourFeeSettingRepository.DeleteManyAsync(hoursSettings);

                // 更新薪资设置中的课时费设置字段（JSON 序列化）
                salarySetting.ClassHourFeeSettings = JsonSerializer.Serialize(input.ClassHourFeeSettings);

                // 映射其他属性（从 DTO 到实体）
                ObjectMapper.Map(input, salarySetting);

                // 更新薪资设置
                await salarySettingRepository.UpdateAsync(salarySetting);

                // 插入新的课时费设置
                if (input.ClassHourFeeSettings != null && input.ClassHourFeeSettings.Any())
                {
                    // 正确映射类型
                    //   var newHourSettings = ObjectMapper.Map<List<ClassHourFeeSetting>>(input.ClassHourFeeSettings);
                    var newHourSettings = ObjectMapper.Map<List<UpdateSalaryDto>, List<ClassHourFeeSetting>>(input.ClassHourFeeSettings);
                    // 设置关联 ID
                    foreach (var item in newHourSettings)
                    {
                        item.SalarySettingId = id; // 确保关联到当前薪资设置
                        await classHourFeeSettingRepository.InsertAsync(item);
                    }
                }

                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "修改薪资信息失败");
                return ApiResult.Fail(ResultCode.Fail, $"修改薪资信息失败: {ex.Message}");
            }
        }
     
        //上课时间表
        public async Task<ApiResult<List<HourDto>>> GetClassHourFeeSettingAsync(Guid SalarySettingId)
        { 
            try
            {
                var list = await classHourFeeSettingRepository.GetQueryableAsync();
                list = list.Where(x => x.SalarySettingId == SalarySettingId);
                var result = list.Select(x => new HourDto
                { 
                    ClassHourDuration = x.ClassHourDuration,
                    ClassHourFee = x.ClassHourFee,
                    AssistantFee = x.AssistantFee
                }).ToList();
                return ApiResult<List<HourDto>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, " 获取课时费信息 列表失败");
                return ApiResult<List<HourDto>>.Fail(ResultCode.Fail, $" 获取课时费信息 列表失败: {ex.Message}");
            }
        }
         
    }
}
