using Educational.StudentsAndParends.Parents;
using Educational.StudentsAndParends.Students;
using Educational.StudentsAndParents.Parents;
using Educational.StudentsAndParents.Students;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.StudentsAndParents.ParentServices
{
	public class ParentService : ApplicationService, IParentServices
	{
		IRepository<Parent, Guid> parentRepository;
		IRepository<Student, Guid> studentRepository;

		public ParentService(IRepository<Parent, Guid> parentRepository, IRepository<Student, Guid> studentRepository)
		{
			this.parentRepository = parentRepository;
			this.studentRepository = studentRepository;
		}

		/// <summary>
		/// 获取家长列表
		/// </summary>
		/// <param name="searchDto"></param>
		/// <returns></returns>
		[ApiExplorerSettings(GroupName = "学员")]
		public async Task<ApiResult<ApiPaging<List<ParentsDto>>>> GetParentList(ParentSearchDto searchDto)
		{
			try
			{
				var parentList =await parentRepository.GetQueryableAsync();
				parentList = parentList.WhereIf(!string.IsNullOrEmpty(searchDto.Phone), x => x.Phone==searchDto.Phone);
				parentList= parentList.WhereIf(!string.IsNullOrEmpty(searchDto.NickName), x => x.NickName.Contains(searchDto.NickName));
				var page = parentList.PageResult(searchDto.PageIndex, searchDto.PageSize);
				var parentsDto = ObjectMapper.Map<List<Parent>, List<ParentsDto>>(page.Queryable.ToList());



				foreach(var item in parentsDto)
				{
					var studentlist = await studentRepository.GetQueryableAsync();
					var studentdto= ObjectMapper.Map<List<Student>, List<StudentsDto>>(studentlist.Where(x=>x.ParentName==item.PardentName).ToList());
					foreach(var item1 in studentdto)
					{
						item.Studentlist += item1.Name + ",";
					}
					item.Studentlist.Trim(',');
				}
				return ApiResult<ApiPaging<List<ParentsDto>>>.Success(ResultCode.Ok, new ApiPaging<List<ParentsDto>>
				{
					TotleCount = page.RowCount,
					TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / searchDto.PageSize),
					Data = parentsDto
				});
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
