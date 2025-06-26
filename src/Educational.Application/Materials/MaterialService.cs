using AutoMapper.Internal.Mappers;
using Educational.Dto.MaterialDtos;
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

namespace Educational.Materials
{
	[ApiExplorerSettings(GroupName ="物料")]
	public class MaterialService : ApplicationService, IMaterialService
	{
		private readonly IRepository<Material, Guid> _materialRepository;
		private readonly ILogger<MaterialService> logger;

		public MaterialService(
			IRepository<Material, Guid> materialRepository, ILogger<MaterialService> logger)
		{
			_materialRepository = materialRepository;
			this.logger = logger;
		}

		/// <summary>
		/// 新增物料
		/// </summary>
		public async Task<ApiResult<MaterialDto>> CreateMaterialAsync(MaterialDto input)
		{
			try
			{
				//创建年级
				var material=ObjectMapper.Map<MaterialDto, Material>(input);
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
		/// 分页获取物料列表
		/// </summary>
		/// <param name="seachDto"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<ApiResult<ApiPaging<List<MaterialDto>>>> GetPagedMaterialsAsync([FromQuery] MaterialSeachDto seachDto)
		{
			try
			{
				//查询
				var material = await _materialRepository.GetQueryableAsync();
				//筛选
				material = material.WhereIf(!string.IsNullOrEmpty(seachDto.MaterialName), x => x.MaterialName.Contains(seachDto.MaterialName));
                material = material.WhereIf(seachDto.MaterialStatus!=null, x => x.Status == seachDto.MaterialStatus);
				var page = material.PageResult(seachDto.PageIndex, seachDto.PageSize);
				var materialDto = ObjectMapper.Map<List<Material>, List<MaterialDto>>(page.Queryable.ToList());
				var result = new ApiPaging<List<MaterialDto>>
				{
					TotleCount = page.RowCount,
					TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / seachDto.PageSize),
					Data = materialDto
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
				material.StockSum=material.StockSum+quantity;
				var result=await _materialRepository.UpdateAsync(material);
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
				material.StockSum=material.StockSum-quantity;

				var result=await _materialRepository.UpdateAsync(material);
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
		public async Task<ApiResult> UpdataStatus(List<Guid> ids,bool status)
		{
			try
			{
				Guid[] idsArray=ids.ToArray();
				foreach(Guid id in idsArray)
				{
					var material=await _materialRepository.GetAsync(id);
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
		/// 修改物料信息
		/// </summary>
		public async Task<ApiResult<MaterialDto>> UpdateMaterialAsync(Guid id, CreateUpdateMaterialDto input)
		{
			try
			{
				var material=await _materialRepository.GetAsync(id);
				if (material == null)
				{
					return ApiResult<MaterialDto>.Fail(ResultCode.Fail, "物料不存在！");
				}
				var result=ObjectMapper.Map(input, material);
				await _materialRepository.UpdateAsync(result);
				return ApiResult<MaterialDto>.Success(ResultCode.Ok, ObjectMapper.Map<Material, MaterialDto>(result));

			}
			catch (Exception ex)
			{
				logger.LogError("修改物料信息出错！" + ex.Message);
				throw;
			}
		}
	}
}
