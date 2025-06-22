using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Staffs
{
    public class StaffServices : ApplicationService, IStaffServices
    {
        private readonly IRepository<StaffInfo,Guid> basicRepository;

        public StaffServices(IRepository<StaffInfo, Guid> basicRepository)
        {
            this.basicRepository = basicRepository;
        }

        public async Task<ApiResult> AddStaff(StaffInfo staffInfo)
        {
            try
            {
                await basicRepository.InsertAsync(staffInfo);
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        public async Task<ApiResult<StaffInfo>> Login([FromQuery]LoginDTO loginDTO)
        {
            try
            {
                // 查找用户
                var staff = await basicRepository.FirstOrDefaultAsync(x => x.StaffAccount == loginDTO.StaffAccount);

                if (staff == null)
                {
                    return ApiResult<StaffInfo>.Fail(ResultCode.Fail, "用户不存在");
                }

                // 验证密码（这里假设密码是明文存储或简单比较，实际建议使用加密哈希对比）
                if (staff.StaffPassword != loginDTO.StaffPassword)
                {
                    return ApiResult<StaffInfo>.Fail(ResultCode.Fail, "密码错误");
                }
                return ApiResult<StaffInfo>.Success(ResultCode.Ok, staff);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
