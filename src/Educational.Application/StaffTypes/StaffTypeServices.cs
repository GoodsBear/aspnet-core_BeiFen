using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.StaffTypes
{
    public class StaffTypeServices : ApplicationService, IStaffTypeServices
    {
        private readonly IRepository<StaffTypeInfo> repository;

        public StaffTypeServices(IRepository<StaffTypeInfo> repository)
        {
            this.repository = repository;
        }
        public async Task<ApiResult<List<ShowStaffTypeDTO>>> GetStaffType()
        {
            var stafftypeinfo=await repository.GetListAsync();
            var stafftypedto=ObjectMapper.Map<List<StaffTypeInfo>, List<ShowStaffTypeDTO>>(stafftypeinfo);
            return ApiResult<List<ShowStaffTypeDTO>>.Success(ResultCode.Ok, stafftypedto);
        }
    }
}
