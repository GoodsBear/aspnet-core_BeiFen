using Educational.Organization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.SpecialSubject
{
    public interface ISpecialSubjectServices: IApplicationService
    { 
       /// <summary>
       /// 通过主键反填
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        Task<ApiResult<SpecialSubjectDto>> GetAsync(Guid id);//保留这个
        /// <summary>
        /// 通过表单查询
        /// </summary>
        /// <param name="seach">当前页面，每页数量</param>
        /// <returns></returns>
        Task<ApiResult<ApiPaging<List<SpecialSubjectandCategory>>>> GetListAsync([FromQuery] SpecialSubjectSearchDto search);
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="input">修改字段</param>
        /// <returns></returns>
        Task<ApiResult<SpecialSubjectDto>> CreateAsync(UpdateSpecialSubjectDto input); 
        /// <summary>
        /// 修改字段
        /// </summary>
        /// <param name="input">修改字段</param>
        /// <returns></returns>
        Task<ApiResult<SpecialSubjectDto>> UpdateAsync(Guid id, SpecialSubjectDto input);
        /// <summary>
        /// 主键删除--批量
        /// </summary> 
        Task<ApiResult> DeleteGuidAsync(List<Guid> guids);
        /// <summary>
        /// 级别下拉表
        /// </summary> 
        Task<ApiResult<List<CategoryModelDto>>> GetCategoryAsync();
        /// <summary>
        /// 级别添加
        /// </summary>  
        Task<ApiResult<CategoryModelDto>> CreateCategoryAsync(UpdateCategoryDto Dto);
        /// <summary>
        /// 专题下拉
        /// </summary>
        Task<ApiResult<List<XiAsepecialSubjectDto>>> XiASpecialSubjectAsync();
    }
}
