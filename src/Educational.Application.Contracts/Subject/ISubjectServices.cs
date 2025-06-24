using Educational.Organization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.Subject
{ 
    public interface ISubjectServices : IApplicationService
    { 
        /// <summary>
        /// 通过主键反填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<SubjectModel>> GetOneAsync(Guid id);
        /// <summary>
        /// 通过表单查询
        /// </summary>
        /// <param name="seach">当前页面，每页数量</param>
        /// <returns></returns>
        Task<ApiResult<ApiPaging<List<SubjectDto>>>> GetListAsync([FromQuery] SubjectSearchDto search); 
        /// <summary>
        /// 添加 Subject
        /// </summary>
        /// <param name="input">修改字段</param>
        /// <returns></returns>
        Task<ApiResult<SubjectDto>> CreateSubjectAsync(UpdateSubjectDto input); 
        /// <summary>
        /// 修改字段
        /// </summary>
        /// <param name="input">修改字段</param>
        /// <returns></returns>
        Task<ApiResult<SubjectModel>> UpdateAsync(Guid id, UpdateSubjectDto input);
        /// <summary>
        /// 主键删除--逻辑删除
        /// </summary> 
         Task<ApiResult> SubjectDeleteAsync(List<Guid> guids); 
        /// <summary>
        /// 科目管理下拉表
        /// </summary> 
        Task<ApiResult<List<XialaSubjectDto>>> GetSubjectAsync();
       
    }
}