using Educational.Dto.Positions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Positions
{
    public class PositionAppService : ApplicationService, IPositionAppService
    {
        IRepository<Position, Guid> positionRep; 
        ILogger<PositionAppService> logger;

        public PositionAppService(IRepository<Position, Guid> positionRep, ILogger<PositionAppService> logger)
        {
            this.positionRep = positionRep;
            this.logger = logger;
        }
        /// <summary>
        /// 创建职位
        /// </summary>
        /// <param name="createPositionDto"> 添加Dto </param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult<PositionsDto>> CreatePosition(CreateUpdatePositionDto createPositionDto)
        {
            try
            {

                //创建职位
                var position = ObjectMapper.Map<CreateUpdatePositionDto, Position>(createPositionDto);
                //  插入数据库
                var result = await positionRep.InsertAsync(position);
                //映射
                var positionsDto = ObjectMapper.Map<Position, PositionsDto>(result);
                //返回
                return ApiResult<PositionsDto>.Success(ResultCode.Ok, positionsDto);
            }
            catch (Exception ex)
            {
                //记录日志
                logger.LogError("职位添加出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="id">根据id查询修改数据</param>
        /// <param name="createPositionDto">修改dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult<PositionsDto>> UpdatePosition(Guid id, CreateUpdatePositionDto createPositionDto)
        {
            try
            {
                var position = await positionRep.GetAsync(id);

                if (position == null)
                {
                    return ApiResult<PositionsDto>.Fail(ResultCode.Fail, "职位不存在！");
                }
                var result = ObjectMapper.Map(createPositionDto, position);
                await positionRep.UpdateAsync(result);
                return ApiResult<PositionsDto>.Success(ResultCode.Ok, ObjectMapper.Map<Position, PositionsDto>(result));
            }
            catch (Exception ex)
            {
                logger.LogError("职位修改出错！" + ex.Message);
                throw;
            }
           
        }
        /// <summary>
        /// 职位列表
        /// </summary>
        /// <param name="searchDto">查询条件</param>
        /// <returns>返回职位列表</returns>
        public async Task<ApiResult<ApiPaging<List<PositionsDto>>>> GetPositionList([FromQuery] PositionSearchDto searchDto)
        {
            try
            {
                // 获取职位列表
                var list = await positionRep.GetQueryableAsync();
                // 职位名称查询
                list = list.WhereIf(!string.IsNullOrEmpty(searchDto.PositionName), x => x.PositionName.Contains(searchDto.PositionName));
                // 使用ABP自带分页方法
                var page = list.PageResult(searchDto.PageIndex, searchDto.PageSize);
                // 映射
                var positionDto = ObjectMapper.Map<List<Position>, List<PositionsDto>>(page.Queryable.ToList());
                var result = new ApiPaging<List<PositionsDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / searchDto.PageSize),
                    Data = positionDto
                };
                return ApiResult<ApiPaging<List<PositionsDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("职位显示报错！" + ex.Message);
                throw;
            }
        }
    }
}

