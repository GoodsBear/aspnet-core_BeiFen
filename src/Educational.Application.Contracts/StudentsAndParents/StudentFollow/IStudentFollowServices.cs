using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Educational.StudentsAndParents.StudentFollow
{
    public interface IStudentFollowServices
    {
        Task<ApiResult<FollowDto>> AddAsync(CreateUpdateFollowDto createUpdateFollowDto);
    }
}
