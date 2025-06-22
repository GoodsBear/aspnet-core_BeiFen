using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Educational.Staffs;


namespace Educational.Staffs
{
    public interface IStaffServices:IApplicationService
    {
        Task<ApiResult<StaffInfo>> AddStaff(StaffInfo staffInfo);
        Task<ApiResult<StaffInfo>> Login(LoginDTO loginDTO);
    }
}
