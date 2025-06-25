using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.StaffTypes
{
    public interface IStaffTypeServices : IApplicationService
    {
        Task<ApiResult<List<ShowStaffTypeDTO>>> GetStaffType();
    }
}
