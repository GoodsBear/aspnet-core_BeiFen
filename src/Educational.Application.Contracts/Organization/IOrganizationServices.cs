using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.Organization
{
    /// <summary>
    /// 组织机构管理接口
    /// </summary>
    public interface IOrganizationServices : IApplicationService
    {
        /// <summary>
        /// 通过主键反填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<OrganizationDto>> GetAsync(Guid id); 
        /// <summary>
        /// 通过表单查询
        /// </summary>
        /// <param name="seach">当前页面，每页数量</param>
        /// <returns></returns>
        Task<ApiResult<ApiPaging<List<LevelDto>>>> GetListAsync([FromQuery] Seach search);
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="input">修改字段</param>
        /// <returns></returns>
         Task<ApiResult<OrganizationDto>> CreateAsync(CreateUpdateOrganizationDto input);

        /// <summary>
        /// 修改字段
        /// </summary>
        /// <param name="input">修改字段</param>
        /// <returns></returns>
        Task<ApiResult<OrganizationDto>> UpdateAsync(Guid id, OrganizationDto input);
        /// <summary>
        /// 主键删除
        /// </summary> 
        Task<ApiResult> DeleteAsync(Guid id);
        /// <summary>
        /// 树形组织机构表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>

        Task<ApiResult<List<OrganizationTreeDto>>> GetTreeAsync([DefaultValue("00000000-0000-0000-0000-000000000000")] Guid parentId);
        /// <summary>
        /// 组织机构级别下拉表
        /// </summary> 
         Task<ApiResult<List<XialaLevelDto>>> GetLevelAsync();
        /// <summary>
        /// 组织机构下拉框
        /// </summary>
        Task<ApiResult<List<OrganizationSelectDto>>> GetOrganizationAsync();
        /// <summary>
        /// 组织机构级别添加
        /// </summary>  
        Task<ApiResult<OrganizationLevelDto>> CreateLevelAsync(CreateUpdateOrganizationLevel Dto);
    }
} 