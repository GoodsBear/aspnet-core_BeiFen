using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Educational.StudentsAndParents.StudentFollow
{
    public interface IFollowServices
    {
        Task<ApiResult<FollowDto>> AddAsync(Guid guid,CreateUpdateFollowDto createUpdateFollowDto);

        Task<ApiResult<List<FollowDto>>> GetFollowList(Guid StudentGuid);

        Task<ApiResult> DelFollow(Guid followGuid, Guid StudentGuid);
    }
}
