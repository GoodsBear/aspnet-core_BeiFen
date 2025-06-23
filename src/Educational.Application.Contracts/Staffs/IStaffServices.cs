using Educational.Enmu;
using Educational.Staffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;


namespace Educational.Staffs
{
    public interface IStaffServices:IApplicationService
    {
        /// <summary>添加员工</summary>
        Task<ApiResult<ShowStaffDTO>> AddStaff(AddorUpdStaffDTO addorUpdStaffDTO);

        /// <summary>员工登录</summary>
        Task<ApiResult<StaffInfo>> Login(LoginDTO loginDTO);

        /// <summary>编辑员工</summary>
        Task<ApiResult<ShowStaffDTO>> UpdateStaff(Guid staffId, AddorUpdStaffDTO addorUpdStaffDTO);

        /// <summary>修改密码</summary>
        Task<ApiResult> UpdateStaffPassword(Guid staffId, string newPassword);

        /// <summary>删除员工</summary>
        Task<ApiResult> DeleteStaff(Guid staffId);

        /// <summary>修改员工状态</summary>
        Task<ApiResult> UpdateStaffStatus(Guid staffId, StaffStatus status);
        /// <summary>分页查询员工信息</summary>
        Task<ApiResult<ApiPaging<List<ShowStaffDTO>>>> GetStaffListAsync(SearchStaffDTO search);
        /// <summary>导出员工信息</summary>
        Task<ApiResult<ExportResult>> GetExportStaffList();

    }
}
