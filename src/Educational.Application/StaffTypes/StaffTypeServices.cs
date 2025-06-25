using Educational.Tools;
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
        private readonly RedisHelp<ShowStaffTypeDTO> redisHelper;

        public StaffTypeServices(IRepository<StaffTypeInfo> repository, RedisHelp<ShowStaffTypeDTO> redisHelper)
        {
            this.repository = repository;
            this.redisHelper = redisHelper;
        }
        /// <summary>
        /// 获取人员类型（集成Redis缓存）
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<ShowStaffTypeDTO>>> GetStaffType()
        {
            // 使用 Redis 缓存
            var staffTypeDtoList = await redisHelper.GetRedisList("StaffType", async () =>
            {
                // 缓存中没有数据时，从数据库获取
                var stafftypeinfo = await repository.GetListAsync();
                var stafftypedto = ObjectMapper.Map<List<StaffTypeInfo>, List<ShowStaffTypeDTO>>(stafftypeinfo);
                return stafftypedto;
            }, 60 * 60); // 缓存1小时

            // 转换为 List 并返回
            return ApiResult<List<ShowStaffTypeDTO>>.Success(ResultCode.Ok, staffTypeDtoList.ToList());
        }
    }
}
