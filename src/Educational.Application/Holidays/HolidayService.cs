using Abp.Application.Services;
using AutoMapper;
using Educational.Courses;
using Educational.Holidays.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Educational.Holidays
{
	/// <summary>
	/// 节假日服务
	/// </summary>
	[ApiExplorerSettings(GroupName = "节假日")]
	public class HolidayService : ApplicationService, IHolidayServices
	{
		private readonly IRepository<Holiday,Guid> holidayRepository;
		IMapper mapper;
		public HolidayService(IRepository<Holiday, Guid> holidayRepository, IMapper mapper)
		{
			this.holidayRepository = holidayRepository;
			this.mapper = mapper;
		}


		/// <summary>
		/// 新增节假日
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<ApiResult<HolidayDto>> CreateAsync(CreateHolidayDto input)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 删除节假日
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<ApiResult> DeleteAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取节单个假日
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<ApiResult<HolidayDto>> GetAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取节假日列表
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<ApiResult<List<HolidayDto>>> GetListAsync(SearchHolidayDto search)
		{
			try
			{
				var holidays=await holidayRepository.GetListAsync();
				holidays = holidays.OrderByDescending(x=>x.Date).ToList();
				var years=DateTime.Now.Year;
				if (search.Year!=null)
				{
					holidays= holidays.Where(x=>x.Date.Year==search.Year).ToList();
				}
				else
				{
                    holidays= holidays.Where(x=>x.Date.Year==years).ToList();
				}
				if (search.HolidayName!=null)
				{
					holidays= holidays.Where(x=>x.HolidayName.Contains(search.HolidayName)).ToList();
				}
				//映射到DTO
				List<HolidayDto> holiday= mapper.Map<List<Holiday>, List<HolidayDto>>(holidays);
				return ApiResult<List<HolidayDto>>.Success(ResultCode.Ok, holiday);
			}
			catch (Exception)
			{

				throw;
			}
		}

		/// <summary>
		/// 修改节假日
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<ApiResult<HolidayDto>> UpdateAsync(Guid id, UpdateHolidayDto input)
		{
			throw new NotImplementedException();
		}
	}
}
