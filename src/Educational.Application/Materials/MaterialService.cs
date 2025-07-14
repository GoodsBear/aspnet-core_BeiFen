using AutoMapper.Internal.Mappers;
using Educational.Dto.MaterialDtos;
using Educational.Dto.Positions;
using Educational.Organization;
using Educational.Positions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;
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
using Volo.Abp.Linq;
using Volo.Abp.Uow;

namespace Educational.Materials
{
	[ApiExplorerSettings(GroupName ="物料")]
	public class MaterialService : ApplicationService, IMaterialService
	{
		private readonly IRepository<Material, Guid> _materialRepository;
		private readonly IRepository<MaterialRecords, Guid> _materialRecordsRepository;
		private readonly IRepository<OrganizationModel, Guid> _organizationRepository;
		private readonly ILogger<MaterialService> logger;

        public MaterialService(
            IRepository<Material, Guid> materialRepository, ILogger<MaterialService> logger, IRepository<OrganizationModel, Guid> organizationRepository, IRepository<MaterialRecords, Guid> materialRecordsRepository)
        {
            _materialRepository = materialRepository;
            this.logger = logger;
            _organizationRepository = organizationRepository;
            _materialRecordsRepository = materialRecordsRepository;
        }
        /// <summary>
        /// 新增物料
        /// </summary>
        public async Task<ApiResult<MaterialDto>> CreateMaterialAsync(CreateUpdateMaterialDto input)
		{
			try
			{
				input.StockSum = 10;
				input.Status = true;
				//创建年级
				var material=ObjectMapper.Map<CreateUpdateMaterialDto, Material>(input);
				//插入数据库
                var result=await _materialRepository.InsertAsync(material);
				//返回
				return ApiResult<MaterialDto>.Success(ResultCode.Ok, ObjectMapper.Map<Material, MaterialDto>(result));
			}
			catch (Exception ex)
			{
				//记录日志
                logger.LogError("物料添加出错！"+ex.Message);
				throw;
			}
		}
		/// <summary>
		/// 修改物料
		/// </summary>
        public async Task<ApiResult<MaterialDto>> UpdateMaterial(Guid id, CreateUpdateMaterialDto createPositionDto)
        {
            try
            {
                var material = await _materialRepository.GetAsync(id);
                if (material == null)
                {
                    return ApiResult<MaterialDto>.Fail(ResultCode.Fail, "此物料不存在！");
                }
                var result = ObjectMapper.Map(createPositionDto, material);
                await _materialRepository.UpdateAsync(result);
                return ApiResult<MaterialDto>.Success(ResultCode.Ok, ObjectMapper.Map<Material, MaterialDto>(result));
            }
            catch (Exception ex)
            {
                logger.LogError("物料修改出错！" + ex.Message);
                throw;
            }

        }
        /// <summary>
        /// 分页获取物料列表
        /// </summary>
        /// <param name="seachDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<ApiPaging<List<MaterialDto>>>> GetPagedMaterialsAsync([FromQuery] MaterialSeachDto seachDto)
		{
			try
			{
                var organizationlist = await _organizationRepository.GetListAsync();
                var materiallist = await _materialRepository.GetListAsync();
                //筛选
                materiallist = materiallist.WhereIf(!string.IsNullOrEmpty(seachDto.MaterialName), x => x.MaterialName.Contains(seachDto.MaterialName)).ToList();
                materiallist = materiallist.WhereIf(seachDto.MaterialStatus != null, x => x.Status == seachDto.MaterialStatus).ToList();
                var list = (from materials in materiallist
                           join school in organizationlist on materials.SchoolId equals school.Id
                           select new MaterialDto
                           {
                               Id = materials.Id,
                               MaterialName = materials.MaterialName,
                               MaterialImage = materials.MaterialImage,
                               MaterialTypeId = materials.MaterialTypeId,
                               MaterialTypeName = Enum.GetName(typeof(MaterialTypeEnum), materials.MaterialTypeId),
                               SchoolId = materials.SchoolId,
                               SchoolName = school.Name,
                               StockSum = materials.StockSum,
                               Status = materials.Status,
                               LastModificationTime = materials.LastModificationTime,
                               LastModifierId = materials.LastModifierId,
                               CreationTime = materials.CreationTime,
                               CreatorId = materials.CreatorId,
                               MterialDescription = materials.MterialDescription,
                             
                           }).ToList();
                var page = list.AsQueryable().PageResult(seachDto.PageIndex, seachDto.PageSize);
				//var  materialdto = ObjectMapper.Map<List<Material>, List<MaterialDto>>(page.Queryable.ToList());


				var result = new ApiPaging<List<MaterialDto>>
				{
					TotleCount = page.RowCount,
					TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / seachDto.PageSize),
					Data = page.Queryable.ToList()
                };
				return ApiResult<ApiPaging<List<MaterialDto>>>.Success(ResultCode.Ok, result);
			}
			catch (Exception ex)
			{
				logger.LogError("物料列表出错！"+ex.Message);
				throw;
			}
		}
		/// <summary>
		/// 入库
		/// </summary>
		/// <param name="id"></param>
		/// <param name="quantity"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpPut]
		public async Task<ApiResult<CreateUpdateMaterialDto>> StockInAsync(Guid id, int quantity)
		{
			try
			{
				
				var material=await _materialRepository.GetAsync(id);
				Material mater=new Material
				{
					 StockSum=material.StockSum+quantity,
				};
				var result=await _materialRepository.UpdateAsync(mater);

                return ApiResult<CreateUpdateMaterialDto>.Success(ResultCode.Ok, ObjectMapper.Map<Material, CreateUpdateMaterialDto>(result));
			}
			catch (Exception ex)
			{
				logger.LogError("物料入库出错！"+ex.Message);
				throw;
			}
		}
		/// <summary>
		/// 出库
		/// </summary>
		/// <param name="id"></param>
		/// <param name="quantity"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpPut]
		public async Task<ApiResult<CreateUpdateMaterialDto>> StockOutAsync(Guid id, int quantity)
		{
			try
			{
				var material=await _materialRepository.GetAsync(id);
                Material mater=new Material
				{
					 StockSum=material.StockSum-quantity,
				};
                var result=await _materialRepository.UpdateAsync(mater);

                return ApiResult<CreateUpdateMaterialDto>.Success(ResultCode.Ok, ObjectMapper.Map<Material, CreateUpdateMaterialDto>(result));
			}
			catch (Exception ex)
			{
				logger.LogError("物料出库出错！"+ex.Message);
				throw;
			}
		}

		/// <summary>
		/// 批量修改物料状态
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpPut]
		[UnitOfWork]
		public async Task<ApiResult> UpdataStatus(List<Guid> ids,bool status)
		{
			try
			{
				foreach(var id in ids)
				{
					var material = await _materialRepository.GetAsync(id);
					material.Status = status;
                    await _materialRepository.UpdateAsync(material);
				}
                return ApiResult.Success(ResultCode.Ok);
			}
			catch (Exception ex)
			{
				logger.LogError("批量修改物料状态出错！" + ex.Message);
				throw;
			}
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
                    var material = await _materialRepository.GetAsync(item);
                    if (material == null)
                    {
                        return ApiResult.Fail(ResultCode.Fail, "物料不存在！");
                    }
                    await _materialRepository.DeleteAsync(material);

                }
                return ApiResult.Success(ResultCode.Ok);

            }
            catch (Exception ex)
            {
                logger.LogError("物料批量删除出错！" + ex.Message);
                throw;
            }
        }
		/// <summary>
		/// 获取物料下拉
		/// </summary>
		/// <returns></returns>
        public async Task<ApiResult<List<MaterialSelectDto>>> GetMaterialAsync()
        {
            try
            {
                var queryable = await _materialRepository.GetListAsync();
                var results = ObjectMapper.Map<List<Material>, List<MaterialSelectDto>>(queryable);
                return ApiResult<List<MaterialSelectDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "物料下拉框获取失败");
                throw;
            }
        }
    }
}
