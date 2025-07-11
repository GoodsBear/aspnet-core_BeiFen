using Abp.Application.Services;
using Educational.Holidays.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Holidays
{
	public interface IHolidayServices:IApplicationService
	{
		//获取单个节假日信息
		Task<ApiResult<HolidayDto>> GetAsync(Guid id);
		//获取所有节假日信息
		Task<ApiResult<List<HolidayDto>>> GetListAsync(SearchHolidayDto search);
		//创建节假日信息
		Task<ApiResult<HolidayDto>> CreateAsync(CreateHolidayDto input);
		//修改节假日信息
		Task<ApiResult<HolidayDto>> UpdateAsync(Guid id, UpdateHolidayDto input);
		//删除节假日信息
		Task<ApiResult> DeleteAsync(Guid id);
	}
}
