using Educational.Dto.MaterialDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Educational.Materials
{
	public interface IMaterialService: IApplicationService
	{
		// 新增物料
		Task<ApiResult<MaterialDto>> CreateMaterialAsync(MaterialDto input);

		// 入库操作
		Task<ApiResult<CreateUpdateMaterialDto>> StockInAsync(Guid id, int quantity);

		// 出库操作
		Task<ApiResult<CreateUpdateMaterialDto>> StockOutAsync(Guid id, int quantity);

		// 分页查询
		Task<ApiResult<ApiPaging<List<MaterialDto>>>> GetPagedMaterialsAsync([FromQuery]MaterialSeachDto seachDto);

		// 更新状态
		Task<ApiResult> UpdataStatus(List<Guid> ids,bool status);
		//更新
        Task<ApiResult<MaterialDto>> UpdateMaterialAsync(Guid id, CreateUpdateMaterialDto  input);
	}
}

