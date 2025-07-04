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
		Task<ApiResult<MaterialDto>> CreateMaterialAsync(CreateUpdateMaterialDto input);


		// 分页查询
		Task<ApiResult<ApiPaging<List<MaterialDto>>>> GetPagedMaterialsAsync([FromQuery]MaterialSeachDto seachDto);

		// 更新状态
		Task<ApiResult> UpdataStatus(List<Guid> ids,bool status);
		// 批量删除
		Task<ApiResult> BatchDelete(List<Guid> ids);
        // 物料修改
        Task<ApiResult<MaterialDto>> UpdateMaterial(Guid id, CreateUpdateMaterialDto createPositionDto);
    }
}

