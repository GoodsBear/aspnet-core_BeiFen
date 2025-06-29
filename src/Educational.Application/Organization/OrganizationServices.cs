using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Organization
{
    [ApiExplorerSettings(GroupName ="组织机构")]
    public class OrganizationServices : ApplicationService, IOrganizationServices
    {
        private readonly IRepository<OrganizationModel, Guid> _organizationRepository;
        private readonly IRepository<OrganizationLevel, Guid> _organizationLevelRepository;
        ILogger<OrganizationServices> logger;

        public OrganizationServices(
            IRepository<OrganizationModel, Guid> organizationRepository,
            IRepository<OrganizationLevel, Guid> organizationLevelRepository,
            ILogger<OrganizationServices> logger)
        {
            _organizationRepository = organizationRepository;
            _organizationLevelRepository = organizationLevelRepository;
            this.logger = logger;
        }

        /// <summary>
        /// 创建组织机构
        /// </summary>
        /// <param name="input">组织机构信息</param>
        /// <returns>创建结果</returns>
        public async Task<ApiResult<OrganizationDto>> CreateAsync(CreateUpdateOrganizationDto input)
        {
            try
            {
                // 验证机构名是否已存在
                var existingOrg = await _organizationRepository.FirstOrDefaultAsync(x => x.Name == input.Name);
                if (existingOrg != null)
                {
                    return ApiResult<OrganizationDto>.Fail(ResultCode.Fail, "机构名已存在");
                } 
                //创建机构 
                var organization = ObjectMapper.Map<CreateUpdateOrganizationDto,OrganizationModel>(input); 
                //插入数据库
                var organizationDto = await _organizationRepository.InsertAsync(organization);
                //映射
                var result = ObjectMapper.Map<OrganizationModel,OrganizationDto>(organizationDto);
                //返回
                return ApiResult<OrganizationDto>.Success(ResultCode.Ok, result); 
            }
            catch (Exception ex)
            {
                return ApiResult<OrganizationDto>.Fail(ResultCode.Fail, $"创建组织机构失败: {ex.Message}");
            }
        } 
        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="id">组织机构ID</param>
        /// <returns>删除结果</returns>
        public async Task<ApiResult> DeleteAsync(Guid id)
        {
            try
            {
                // 检查组织机构是否存在
                var organization = await _organizationRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (organization == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "组织机构不存在");
                }
                // 检查是否有子机构
                var childOrgs = await _organizationRepository.GetListAsync(x => x.PartentedId == id);
                if (childOrgs.Any())
                {
                    return ApiResult.Fail(ResultCode.Fail, "该机构下存在子机构，无法删除");
                }
                // 执行删除
                await _organizationRepository.DeleteAsync(id);
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail(ResultCode.Fail, $"删除组织机构失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据ID获取组织机构--反填
        /// </summary>
        /// <param name="id">组织机构ID</param>
        /// <returns>组织机构信息</returns>
        public async Task<ApiResult<OrganizationDto>> GetAsync(Guid id)
        {
            try
            {
                var organization = await _organizationRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (organization == null)
                {
                    return ApiResult<OrganizationDto>.Fail(ResultCode.Fail, "组织机构不存在");
                }

                var result = ObjectMapper.Map<OrganizationModel, OrganizationDto>(organization);
                return ApiResult<OrganizationDto>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                return ApiResult<OrganizationDto>.Fail(ResultCode.Fail, $"获取组织机构失败: {ex.Message}");
            }
        }
        /// <summary>
        /// 分页查询组织机构列表
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns>分页结果</returns>
        public async Task<ApiResult<ApiPaging<List<LevelDto>>>> GetListAsync([FromQuery]Seach search)
        {
            try
            {
                // 构建查询
                var organizations = await _organizationRepository.GetQueryableAsync();
                var levels = await _organizationLevelRepository.GetQueryableAsync();
                var linq = from org in organizations
                           join level in levels
                           on org.LevelId equals level.Id
                           select new LevelDto
                           {
                               Id = org.Id,
                               LevelName = level.Name,
                               Name = org.Name,
                               LevelId = org.LevelId,
                               PartentedId=org.PartentedId,
                               ShortName=org.ShortName,
                               ContactPerson=org.ContactPerson,
                               Phone=org.Phone,
                               Fax= org.Fax,
                               Email=   org.Email,
                               SortOrder=org.SortOrder,
                               IsActive=org.IsActive,
                               Description=org.Description,
                               DeleterId=org.DeleterId,
                               DeletionTime=org.DeletionTime
                           };
                //按照权重进行排序
                linq = linq.OrderByDescending(x=>x.SortOrder);
                // 使用ABP自带分页方法 
                var page = linq.PageResult(search.PageIndex, search.PageSize);
                // 映射
                //var organizationDto = ObjectMapper.Map<List<>,List<LevelDto>>(page);
                var result = new ApiPaging<List<LevelDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / search.PageSize),
                    Data = linq.Skip((search.PageIndex-1)* search.PageSize).Take(search.PageSize).ToList()
                };
                return ApiResult<ApiPaging<List<LevelDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("获取组织机构列表出错: " + ex.Message);
                throw;
            }
        }  

		/// <summary>
		/// 更新组织机构
		/// </summary>
		/// <param name="id">组织机构ID</param>
		/// <param name="input">更新信息</param>
		/// <returns>更新结果</returns>
		public async Task<ApiResult<OrganizationDto>> UpdateAsync(Guid id, OrganizationDto input)
        {
            try
            {
                // 检查组织机构是否存在
                var organization = await _organizationRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (organization == null)
                {
                    return ApiResult<OrganizationDto>.Fail(ResultCode.Fail, "组织机构不存在");
                }

                // 检查机构名是否重复（排除自己）
                var existingOrg = await _organizationRepository.FirstOrDefaultAsync(x => x.Name == input.Name && x.Id != id);
                if (existingOrg != null)
                {
                    return ApiResult<OrganizationDto>.Fail(ResultCode.Fail, "机构名已存在");
                }

                // 验证上级机构是否存在（不能设置自己为上级）
                if (input.PartentedId != Guid.Empty)
                {
                    if (input.PartentedId == id)
                    {
                        return ApiResult<OrganizationDto>.Fail(ResultCode.Fail, "不能设置自己为上级机构");
                    }
                    var parentOrg = await _organizationRepository.FirstOrDefaultAsync(x => x.Id == input.PartentedId);
                    if (parentOrg == null)
                    {
                        return ApiResult<OrganizationDto>.Fail(ResultCode.Fail, "上级机构不存在");
                    }
                } 
                // 更新组织机构信息
                organization.Name = input.Name;
                organization.LevelId = input.LevelId;
                organization.PartentedId = input.PartentedId;
                organization.ShortName = input.ShortName;
                organization.ContactPerson = input.ContactPerson;
                organization.Phone = input.Phone;
                organization.Fax = input.Fax;
                organization.Email = input.Email;
                organization.SortOrder = input.SortOrder;
                organization.IsActive = input.IsActive;
                organization.Description = input.Description;
                await _organizationRepository.UpdateAsync(organization);
                // 返回更新后的组织机构信息
                var result = ObjectMapper.Map<OrganizationModel, OrganizationDto>(organization);
                return ApiResult<OrganizationDto>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                return ApiResult<OrganizationDto>.Fail(ResultCode.Fail, $"更新组织机构失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 组织机构级别下拉表
        /// </summary> 
        public async Task<ApiResult<List<XialaLevelDto>>> GetLevelAsync()
        { 
            try
            { 
                // 正确获取可查询接口
                var queryable = await  _organizationLevelRepository.GetListAsync();
                // 正确映射集合类型
                var results = ObjectMapper.Map<List<OrganizationLevel>, List<XialaLevelDto>>(queryable); 
                //  返回成功结果
                return ApiResult<List<XialaLevelDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                // 添加日志记录
                Logger.LogError(ex, "组织机构级别获取失败");
                throw;
            }
        }
        /// <summary>
        /// 获取组织机构下拉框
        /// </summary>
        public async Task<ApiResult<List<OrganizationSelectDto>>> GetOrganizationAsync()
        {
            try
            {
                var queryable = await _organizationRepository.GetListAsync();
                var results = ObjectMapper.Map<List<OrganizationModel>, List<OrganizationSelectDto>>(queryable);
                return ApiResult<List<OrganizationSelectDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "组织机构下拉框获取失败");
                throw;
            }
        }
        /// <summary>
        /// 树形组织机构表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>

        public async Task<ApiResult<List<OrganizationTreeDto>>> GetTreeAsync([DefaultValue("00000000-0000-0000-0000-000000000000")]Guid parentId)
        {
            if (parentId == null)
            { 
                parentId = Guid.Empty;
            }
            var allOrganizations = await _organizationRepository.GetListAsync();

            var roots = allOrganizations.Where(o => o.PartentedId == parentId).OrderBy(x => x.SortOrder).ToList();
            var tree = roots.Select(MapToTreeDto).ToList();

            // 构建一个查找表，按父ID分组 
            var orgLookup = allOrganizations
                .ToLookup(o => o.PartentedId as Guid?); 
            // 递归构建子树
            foreach (var item in tree)
            {
                BuildTree(item, orgLookup);
            }

            return ApiResult<List<OrganizationTreeDto>>.Success(ResultCode.Ok, tree);
        } 

        /// <summary>
        /// 组织机构级别添加
        /// </summary> 
        public async Task<ApiResult<OrganizationLevelDto>> CreateLevelAsync(CreateUpdateOrganizationLevel Dto)
        {
            try
            {
                // 验证机构名是否已存在
                var existingOrg = await _organizationLevelRepository.FirstOrDefaultAsync(x => x.Name == Dto.Name);
                if (existingOrg != null)
                {
                    return ApiResult<OrganizationLevelDto>.Fail(ResultCode.Fail, "机构名已存在");
                }
                //创建机构 
                var organization = ObjectMapper.Map<CreateUpdateOrganizationLevel, OrganizationLevel>(Dto);
                //插入数据库
                var organizationDto = await _organizationLevelRepository.InsertAsync(organization);
                //映射
                var result = ObjectMapper.Map<OrganizationLevel, OrganizationLevelDto>(organizationDto);
                //返回
                return ApiResult<OrganizationLevelDto>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("组织机构级别add失败" + ex.Message);
                return ApiResult<OrganizationLevelDto>.Fail(ResultCode.Fail, $"组织机构级别add失败: {ex.Message}");
            }
        }
        private void BuildTree(OrganizationTreeDto dto, ILookup<Guid?, OrganizationModel> lookup)
        {
            var children = lookup[dto.Id].OrderBy(x => x.SortOrder).ToList();
            dto.Chlidren = children.Select(entity =>
            {
                var childDto = ObjectMapper.Map<OrganizationModel,OrganizationTreeDto>(entity);
                BuildTree(childDto, lookup); // 递归调用
                return childDto;
            }).ToList();
        } 
        private OrganizationTreeDto MapToTreeDto(OrganizationModel entity)
        {
            var dto = ObjectMapper.Map<OrganizationModel, OrganizationTreeDto>(entity);
            dto.Chlidren = new List<OrganizationTreeDto>(); // 初始化
            return dto;
        }

       
    }
}
