using Educational.ClassSchedule.DTO;
using Educational.ClassSchedule.Update;
using Educational.Subject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Educational.ClassSchedule
{
    public class ConflictModelServices : ApplicationService, IConflictModelServices
    {
        ILogger<ConflictModelServices> _logger;
        IRepository<ConflictModel, Guid> _conflictModelRepository;//冲突

        public ConflictModelServices(ILogger<ConflictModelServices> logger, IRepository<ConflictModel, Guid> conflictModelRepository)
        {
            _logger = logger;
            _conflictModelRepository = conflictModelRepository;
        }
        //add
        public Task<ApiResult<ConflictModelDto>> CreateClassScheduleAsync(List<UpdateConflictModel> input)
        {
            throw new NotImplementedException();
            //try
            //{ 
            //    ///导入
            //    //返回
            //    return ApiResult<ConflictModelDto>.Success(ResultCode.Ok, result);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("冲突添加异常"+ ex.Message);
            //    return ApiResult<ConflictModelDto>.Success(ResultCode.Ok,"冲突添加异常: "+ex.Message);
            //}
        }
        //show 
        public async Task<ApiResult<ApiPaging<List<ConflictModelDto>>>> GetConfilcListAsync(ConflictSearchDto search)
        {
            try
            {
                
                var conflictinfo = await _conflictModelRepository.GetQueryableAsync();
                conflictinfo = conflictinfo.Where(x=>x.ClassScheduleId==search.Id);
                var page = conflictinfo.PageResult(search.PageIndex, search.PageSize);
                var linq= ObjectMapper.Map<List<ConflictModel>, List<ConflictModelDto>>(page.Queryable.ToList());
                var result = new ApiPaging<List<ConflictModelDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / search.PageSize),
                    Data = linq.ToList()
                };
                //返回
                return ApiResult<ApiPaging<List<ConflictModelDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("冲突显示异常" + ex.Message);
                return ApiResult<ApiPaging<List<ConflictModelDto>>>.Fail(ResultCode.Fail, $"冲突显示异常: {ex.Message}");
            }
        }
        //冲突检查措施
    }
}
