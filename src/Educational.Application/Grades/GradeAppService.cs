using Educational.Classgrade;
using Educational.Dto.Grades;
using Educational.Dto.Positions;
using Educational.Positions;
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

namespace Educational.Grades
{
    [ApiExplorerSettings(GroupName = "年级")]
    public class GradeAppService : ApplicationService, IGradeAppService
    {
        IRepository<Grade, Guid> gradeRep;
        ILogger<GradeAppService> logger;

        public GradeAppService(IRepository<Grade, Guid> gradeRep, ILogger<GradeAppService> logger)
        {
            this.gradeRep = gradeRep;
            this.logger = logger;
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
                    var position = await gradeRep.GetAsync(item);
                    if (position == null)
                    {
                        return ApiResult.Fail(ResultCode.Fail, "年级不存在！");
                    }
                    await gradeRep.DeleteAsync(position);

                }
                return ApiResult.Success(ResultCode.Ok);

            }
            catch (Exception ex)
            {
                logger.LogError("年级批量删除出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 创建年级
        /// </summary>
        /// <param name="createUpdateGradeDto">添加Dto</param>
        /// <returns>返回受影响行数</returns>
        public async  Task<ApiResult<GradeDto>> CreatePosition(CreateUpdateGradeDto createUpdateGradeDto)
        {
            try
            {

                //创建年级
                var grade = ObjectMapper.Map<CreateUpdateGradeDto, Grade>(createUpdateGradeDto);
                //  插入数据库
                var result = await gradeRep.InsertAsync(grade);
                //映射
                var gradeDto = ObjectMapper.Map<Grade, GradeDto>(result);
                //返回
                return ApiResult<GradeDto>.Success(ResultCode.Ok, gradeDto);
            }
            catch (Exception ex)
            {
                //记录日志
                logger.LogError("职位添加出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 年级列表
        /// </summary>
        /// <param name="searchDto">查询条件</param>
        /// <returns>返回年级列表</returns>
        public async  Task<ApiResult<ApiPaging<List<GradeDto>>>> GetPositionList([FromQuery] GradeSearchDto searchDto)
        {
            try
            {
                var list = await gradeRep.GetQueryableAsync();
                list = list.WhereIf(!string.IsNullOrEmpty(searchDto.GradeName), x => x.GradeName.Contains(searchDto.GradeName));
                list = list.WhereIf(!string.IsNullOrEmpty(searchDto.EnrollYear), x=>x.EnrollYear > DateTime.Parse(searchDto.EnrollYear));
                list = list.WhereIf(!string.IsNullOrEmpty(searchDto.EnrollYear), x => x.EnrollYear <= DateTime.Parse(searchDto.EnrollYear));
                var page = list.PageResult(searchDto.PageIndex, searchDto.PageSize);
                var gradeDto = ObjectMapper.Map<List<Grade>, List<GradeDto>>(page.Queryable.ToList());
                var result = new ApiPaging<List<GradeDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / searchDto.PageSize),
                    Data = gradeDto
                };
                return ApiResult<ApiPaging<List<GradeDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("年级显示报错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 修改年级
        /// </summary>
        /// <param name="id">根据id查询条件</param>
        /// <param name="createUpdateGradeDto">修改dto</param>
        /// <returns>返回受影响行数</returns>
        public async  Task<ApiResult<GradeDto>> UpdatePosition(Guid id, CreateUpdateGradeDto createUpdateGradeDto)
        {
            try
            {
                var grade = await gradeRep.GetAsync(id);
                if (grade == null)
                {
                    return ApiResult<GradeDto>.Fail(ResultCode.Fail, "年级不存在！");
                }
                var result = ObjectMapper.Map(createUpdateGradeDto, grade);
                await gradeRep.UpdateAsync(result);
                return ApiResult<GradeDto>.Success(ResultCode.Ok, ObjectMapper.Map<Grade, GradeDto>(result));
            }
            catch (Exception ex)
            {
                logger.LogError("年级修改出错！" + ex.Message);
                throw;
            }
        }
    }
}
