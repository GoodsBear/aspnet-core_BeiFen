using Educational.Dto.MaterialRecordsDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.MaterialRetords
{
	public interface IMaterialRetordService:IApplicationService
	{
		//获取出入库列表
		Task<ApiResult<ApiPaging<List<MaterialRecordsDto>>>> GetMaterialRetordList([FromQuery] SearchMaterialRecordsDto searchDto);
		//入库
		Task<ApiResult<MaterialRecordsDto>> MaterialRetordIn(CreateUpdateMaterialRecordsDto createUpdateMaterialRecordsDto);
		//出库
        Task<ApiResult<MaterialRecordsDto>> MaterialRetordOut(CreateUpdateMaterialRecordsDto createUpdateMaterialRecordsDto);
	}
}
