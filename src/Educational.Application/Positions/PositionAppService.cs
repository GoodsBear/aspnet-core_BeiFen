using Educational.Dto.Positions;
using Educational.Staffs;
using Educational.Tools;
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
    [ApiExplorerSettings(GroupName ="职位")]
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
        /// <summary>
        /// 导出职位列表
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns>返回导出结果</returns>
        public async Task<ApiResult<ExportResult>> GetExportPositionList()
        {
            // 获取员工数据源
            var position = await positionRep.GetQueryableAsync();
            // 映射成DTO
            var positiondto = ObjectMapper.Map<List<Position>, List<ExportPositionDto>>(position.ToList());
            // 调用导出帮助类生成 Excel
            var fileBytes = ExcelExporter.Export(positiondto, "职位信息", "职位信息表");
            // 返回导出结果
            return ApiResult<ExportResult>.Success(ResultCode.Ok, new ExportResult
            {
                FileName = $"职位信息_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
                FileContent = fileBytes
            });
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">批删数组</param>
        /// <returns>返回受影响行数</returns>
        [HttpDelete]
        public async Task<ApiResult> BatchDelete(List<Guid> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var position = await positionRep.GetAsync(item);
                    if (position == null)
                    {
                        return ApiResult.Fail(ResultCode.Fail, "职位不存在！");
                    }
                    await positionRep.DeleteAsync(position);

                }
                return ApiResult.Success(ResultCode.Ok);

            }
            catch (Exception ex)
            {
                logger.LogError("职位批量删除出错！" + ex.Message);
                throw;
            }
        }
    }
}

