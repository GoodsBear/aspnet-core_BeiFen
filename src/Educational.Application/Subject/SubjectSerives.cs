using AutoMapper.Internal.Mappers;
using Educational.Dto.Positions;
using Educational.Organization;
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
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Educational.Subject
{
    [ApiExplorerSettings(GroupName = "科目")]
    public class SubjectSerives : ApplicationService, ISubjectServices
    {
        private readonly IRepository<Educational.Subject.SubjectModel, Guid> _subjectRepository;
        ILogger<SubjectSerives> logger;

        public SubjectSerives(IRepository<Educational.Subject.SubjectModel, Guid> subjectRepository, ILogger<SubjectSerives> logger)
        {
            _subjectRepository = subjectRepository;
            this.logger = logger;
        }
        /// <summary>
        /// 科目名称添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult<SubjectDto>> CreateSubjectAsync(UpdateSubjectDto input)
        {
            try
            {
                // 科目名称是否已存在
                var existingSub =  await _subjectRepository.FirstOrDefaultAsync(x => x.SubjectName == input.SubjectName);
                if (existingSub != null)
                {
                    return ApiResult<SubjectDto>.Fail(ResultCode.Fail, "科目名称已存在");
                }
                //创建机构 
                var subjectlist = ObjectMapper.Map<UpdateSubjectDto, Educational.Subject.SubjectModel>(input);
                //插入数据库
                var subjectDto = await _subjectRepository.InsertAsync(subjectlist);
                //映射
                var result = ObjectMapper.Map<Educational.Subject.SubjectModel, SubjectDto>(subjectDto);
                //返回
                return ApiResult<SubjectDto>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("创建科目名称失败"+ex);
                return ApiResult<SubjectDto>.Fail(ResultCode.Fail, $"创建科目名称失败: {ex.Message}");
            }
        }
        /// <summary>
        /// 批量删除---科目名称
        /// </summary> 
        public async Task<ApiResult> SubjectDeleteAsync(List<Guid> guids)
        {
            try
            {
                Guid[] ids = guids.ToArray();

                foreach (var id in ids)
                {
                    var course =  await _subjectRepository.FirstOrDefaultAsync(x => x.Id == id); 
                    //删除
                    await _subjectRepository.DeleteAsync(course);
                }
                return ApiResult.Success(ResultCode.Ok); 
            }
            catch (Exception ex)
            {
                logger.LogError("批量删除subject失败" + ex);
                return ApiResult<SubjectDto>.Fail(ResultCode.Fail, $"批量删除subject失败: {ex.Message}");
            }
        }
         
        /// <summary>
        /// 反填---科目名称
        /// </summary> 

        public async Task<ApiResult<Educational.Subject.SubjectModel>> GetOneAsync(Guid id)
        {
            try
            {  
             var course =  await _subjectRepository.FirstOrDefaultAsync(x => x.Id == id);
                return ApiResult<Educational.Subject.SubjectModel>.Success(ResultCode.Ok, course);  
            }
            catch (Exception ex)
            {
                logger.LogError("反填---科目名称失败" + ex);
                return ApiResult<Educational.Subject.SubjectModel>.Fail(ResultCode.Fail, $"反填---科目名称异常: {ex.Message}");
            }
        }
        /// <summary>
        /// 查询分页---科目名称
        /// 
        /// </summary> 

        public async Task<ApiResult<ApiPaging<List<SubjectDto>>>> GetListAsync([FromQuery] SubjectSearchDto search)
        {
            try
            {
                // 构建查询
                var existingSub =  await _subjectRepository.GetQueryableAsync();
                // 职位名称查询
                existingSub = existingSub.WhereIf(!string.IsNullOrEmpty(search.SubjectName), x => x.SubjectName.Contains(search.SubjectName));
                // 使用ABP自带分页方法 
                var page = existingSub.PageResult(search.PageIndex, search.PageSize);
                // 映射 
                var subjectDto = ObjectMapper.Map<List<Educational.Subject.SubjectModel>, List<SubjectDto>>(page.Queryable.ToList());
                var result = new ApiPaging<List<SubjectDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / search.PageSize),
                    Data = subjectDto
                };
                return ApiResult<ApiPaging<List<SubjectDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("查询分页---科目名称异常" + ex);
                return ApiResult<ApiPaging<List<SubjectDto>>>.Fail(ResultCode.Fail, $"查询分页---科目名称异常: {ex.Message}");
            }

        }
        /// <summary>
        /// 下拉框
        /// </summary>
        public async Task<ApiResult<List<XialaSubjectDto>>> GetSubjectAsync()
        {
            try
            { 
                // 正确获取可查询接口
                var queryable = await _subjectRepository.GetListAsync();
                // 正确映射集合类型
                var results = ObjectMapper.Map<List<Educational.Subject.SubjectModel>, List<XialaSubjectDto>>(queryable);
                //  返回成功结果
                return ApiResult<List<XialaSubjectDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                logger.LogError("下拉框---科目名称异常" + ex);
                throw;
            }
        }
        /// <summary>
        /// 修改 
        /// <summary>

        public async Task<ApiResult<Educational.Subject.SubjectModel>> UpdateAsync(Guid id, UpdateSubjectDto input)
        {
            try
            {
                // 检查科目是否存在
                var organization =  await  _subjectRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (organization == null)
                {
                    return ApiResult<Educational.Subject.SubjectModel>.Fail(ResultCode.Fail, "科目不存在");
                } 
                // 检查机构名是否重复（排除自己）
                var existingOrg = await _subjectRepository.FirstOrDefaultAsync(x => x.SubjectName == input.SubjectName && x.Id != id);
                if (existingOrg != null)
                {
                    return ApiResult<Educational.Subject.SubjectModel>.Fail(ResultCode.Fail, "科目已存在");
                }
                 ObjectMapper.Map(input, organization);
                //organization.Id = id;
                // 返回更新后的组织机构信息
               var result = ObjectMapper.Map<UpdateSubjectDto, Educational.Subject.SubjectModel> (input);
                //result.Id = id;
                var a = await _subjectRepository.UpdateAsync(organization);
                if (a == null)
                {
                    return ApiResult<Educational.Subject.SubjectModel>.Fail(ResultCode.Fail, "更新科目失败"); 
                }
                return ApiResult<Educational.Subject.SubjectModel>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                 logger.LogError(ex, $"更新科目失败. ID={id}, 科目名={input.SubjectName}", id, input.SubjectName);
                return ApiResult<Educational.Subject.SubjectModel>.Fail(ResultCode.Fail, $"更新科目失败: {ex.Message}");
            }
        }
         

        
    }
}
