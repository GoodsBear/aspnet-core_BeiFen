using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StudentsAndParents.Stores
{
    public interface IStoreServices
    {
        Task<ApiResult<StoreDto>> AddAsync(CreateUpdateStoreDto createUpdateDto,Guid guid, int AddOrRecude);

        Task<ApiResult<ApiPaging<List<StoreDto>>>> PageAsync(Guid guid, SearchDto searchDto);
    }
}
