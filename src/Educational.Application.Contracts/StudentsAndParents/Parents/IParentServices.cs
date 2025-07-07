using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.StudentsAndParents.Parents
{
	public interface IParentServices:IApplicationService
	{
		Task<ApiResult<ApiPaging<List<ParentsDto>>>> GetParentList(ParentSearchDto searchDto);
	}
}
