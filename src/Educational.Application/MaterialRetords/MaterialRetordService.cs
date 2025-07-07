using Educational.Dto.MaterialRecordsDtos;
using Educational.Materials;
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
using Educational.Staffs;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Volo.Abp.Uow;
using Educational.StudentsAndParends.Students;

namespace Educational.MaterialRetords
{
	[ApiExplorerSettings(GroupName = "物料")]
	public class MaterialRetordService : ApplicationService, IMaterialRetordService
	{
		private readonly IRepository<MaterialRecords, Guid> _materialRecordsRepository;
		private readonly IRepository<Material, Guid> _materialRepository;
		private readonly IRepository<StaffInfo, Guid> _staffRepository;
		private readonly IRepository<Student, Guid> _studentRepository;
		private readonly ILogger<MaterialRetordService> logger;

		public MaterialRetordService(IRepository<MaterialRecords, Guid> materialRecordsRepository, IRepository<Material, Guid> materialRepository, ILogger<MaterialRetordService> logger, IRepository<StaffInfo, Guid> staffRepository, IRepository<Student, Guid> studentRepository)
		{
			_materialRecordsRepository = materialRecordsRepository;
			_materialRepository = materialRepository;
			this.logger = logger;
			_staffRepository = staffRepository;
			_studentRepository = studentRepository;
		}

		/// <summary>
		/// 获取物料变动列表
		/// </summary>
		/// <param name="searchDto">查询dto</param>
		/// <returns>返回 获取物料变动列表</returns>
		public async Task<ApiResult<ApiPaging<List<MaterialRecordsDto>>>> GetMaterialRetordList([FromQuery] SearchMaterialRecordsDto searchDto)
		{
			var retorlist = await _materialRecordsRepository.GetQueryableAsync();
			if (searchDto.StaffId != null)
			{
				retorlist = retorlist.Where(x => x.StaffId == searchDto.StaffId);
			}
			if (searchDto.MaterialId != null)
			{
				retorlist = retorlist.Where(x => x.MaterialId == searchDto.MaterialId);
			}
			if (searchDto.StudentId != null)
			{
				retorlist = retorlist.Where(x => x.StudentId == searchDto.StudentId);
			}
			if (searchDto.ChangeType != null)
			{
				retorlist = retorlist.Where(x => x.ChangeType == searchDto.ChangeType);
			}
			if (!string.IsNullOrEmpty(searchDto.startTime))
			{
				retorlist = retorlist.Where(x => x.ChangeDate >= DateTime.Parse(searchDto.startTime));
			}
			if (!string.IsNullOrEmpty(searchDto.endTime))
			{
				retorlist = retorlist.Where(x => x.ChangeDate < DateTime.Parse(searchDto.endTime).AddDays(1));
			}
			var page = retorlist.PageResult(searchDto.PageIndex, searchDto.PageSize);
			var retords = ObjectMapper.Map<List<MaterialRecords>, List<MaterialRecordsDto>>(page.Queryable.ToList());
			var material = _materialRepository.GetListAsync().Result;
			var staffinfo = _staffRepository.GetListAsync().Result;
			var student = _studentRepository.GetListAsync().Result;
			foreach (var item in retords)
			{
				item.MaterialName = material.Where(x => x.Id == item.MaterialId).ToList().FirstOrDefault().MaterialName;
				item.StaffName = staffinfo.Where(x => x.Id == item.StaffId).ToList().FirstOrDefault().StaffName;
				item.StudentName = student.Where(x => x.Id == item.StudentId).ToList().FirstOrDefault().Name;
				item.ChangeTypeName = Enum.GetName(typeof(ChangeEnum), item.ChangeType);

            }
			var result = new ApiPaging<List<MaterialRecordsDto>>
			{
				TotleCount = page.RowCount,
				TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / searchDto.PageSize),
				Data = retords
			};
			return ApiResult<ApiPaging<List<MaterialRecordsDto>>>.Success(ResultCode.Ok, result);


		}
		/// <summary>
		/// 物料入库
		/// </summary>
		/// <param name="createUpdateMaterialRecordsDto"></param>
		/// <returns></returns>
		[UnitOfWork]
		public async Task<ApiResult<MaterialRecordsDto>> MaterialRetordIn(CreateUpdateMaterialRecordsDto createUpdateMaterialRecordsDto)
		{
			try
			{

				var materialRecord = new MaterialRecords
				{
					MaterialId = createUpdateMaterialRecordsDto.MaterialId,
					ChangeSum = createUpdateMaterialRecordsDto.ChangeSum,
					StaffId = createUpdateMaterialRecordsDto.StaffId,
					ChangeType = ChangeEnum.入库,
					Reason = createUpdateMaterialRecordsDto.Reason
				};
				var result = await _materialRecordsRepository.InsertAsync(materialRecord);
				var material=await _materialRepository.GetAsync(createUpdateMaterialRecordsDto.MaterialId);
				material.StockSum+=createUpdateMaterialRecordsDto.ChangeSum;
				await _materialRepository.UpdateAsync(material);
				return ApiResult<MaterialRecordsDto>.Success(ResultCode.Ok, ObjectMapper.Map<MaterialRecords, MaterialRecordsDto>(result));
			}
			catch (Exception)
			{

				throw;
			}
		}
		/// <summary>
		/// 物料出库
		/// </summary>
		/// <param name="createUpdateMaterialRecordsDto"></param>
		/// <returns></returns>
		[UnitOfWork]
		public async Task<ApiResult<MaterialRecordsDto>> MaterialRetordOut(CreateUpdateMaterialRecordsDto createUpdateMaterialRecordsDto)
		{
			try
			{
				var materialRecord = new MaterialRecords
				{
					MaterialId = createUpdateMaterialRecordsDto.MaterialId,
					ChangeSum = createUpdateMaterialRecordsDto.ChangeSum,
					StaffId = createUpdateMaterialRecordsDto.StaffId,
                    StudentId = createUpdateMaterialRecordsDto.StudentId,
					ChangeType = ChangeEnum.出库,
					Reason = createUpdateMaterialRecordsDto.Reason
				};
				var result = await _materialRecordsRepository.InsertAsync(materialRecord);
				var material = await _materialRepository.GetAsync(createUpdateMaterialRecordsDto.MaterialId);
				if (material.StockSum< createUpdateMaterialRecordsDto.ChangeSum)
				{
					return ApiResult<MaterialRecordsDto>.Fail(ResultCode.Fail, "库存不足！");
				}
				material.StockSum -= createUpdateMaterialRecordsDto.ChangeSum;
				await _materialRepository.UpdateAsync(material);
				return ApiResult<MaterialRecordsDto>.Success(ResultCode.Ok, ObjectMapper.Map<MaterialRecords, MaterialRecordsDto>(result));
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
