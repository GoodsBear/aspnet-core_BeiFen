using Educational.StudentsAndParends.Students.Follow;
using Educational.StudentsAndParents.StudentFollow;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.StudentsAndParents.StudentServices
{
    [ApiExplorerSettings(GroupName ="跟进")]
    public class FollowServices : ApplicationService, IFollowServices
    {
        private readonly IRepository<Follow> followrepository;
        private readonly IRepository<StudentFollowRelation> studentfollowrepository;
        private readonly ILogger<FollowServices> logger;

        public FollowServices(IRepository<Follow> followrepository,IRepository<StudentFollowRelation> studentfollowrepository, ILogger<FollowServices> logger)
        {
            this.followrepository = followrepository;
            this.studentfollowrepository = studentfollowrepository;
            this.logger = logger;
        }
        /// <summary>
        /// 新增学员跟进
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="createUpdateFollowDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<FollowDto>> AddAsync(Guid guid, CreateUpdateFollowDto createUpdateFollowDto)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var data = ObjectMapper.Map<CreateUpdateFollowDto, Follow>(createUpdateFollowDto);

                    await followrepository.InsertAsync(data);

                    MiddleCreateUpateDto middleCreateUpateDto = new MiddleCreateUpateDto()
                    {
                        StudentId = guid,
                        FollowId = data.Id
                    };
                    var result = ObjectMapper.Map<MiddleCreateUpateDto, StudentFollowRelation>(middleCreateUpateDto);
                    var query = await studentfollowrepository.InsertAsync(result);
                    tran.Complete();
                    if(query != null)
                    {
                        return  ApiResult<FollowDto>.Success(ResultCode.Ok,ObjectMapper.Map<Follow, FollowDto>(data));
                    }
                    else
                    {
                        return  ApiResult<FollowDto>.Fail(ResultCode.Fail, "添加跟进失败");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("添加跟进出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 删除学员跟进
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpDelete]
        public async Task<ApiResult> DelFollow(Guid followGuid,Guid StudentGuid)
        {
            try
            {
                await followrepository.DeleteAsync(d=>d.Id == followGuid);

                await studentfollowrepository.DeleteAsync(d => d.FollowId == followGuid && d.StudentId == StudentGuid);

                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("学员跟进删除出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 学员跟进列表
        /// </summary>
        [HttpGet]
        public async Task<ApiResult<List<FollowDto>>> GetFollowList(Guid StudentGuid)
        {
            try
            {
                // 获取学员跟进记录
                var list = await studentfollowrepository.GetListAsync(d => d.StudentId == StudentGuid);
                if (list == null || list.Count == 0)
                {
                    return ApiResult<List<FollowDto>>.Fail(ResultCode.Fail, "没有跟进记录");
                }
                list = list.OrderByDescending(d => d.CreationTime).ToList();

                var followDtos = new List<FollowDto>();
                // 遍历学员跟进记录，获取跟进详情
                foreach (var item in list)
                {
                    // 根据跟进ID获取跟进详情
                    var follow = await followrepository.GetAsync(d => d.Id == item.FollowId);
                    if (follow != null)
                    {
                        // 将跟进详情映射到FollowDto并加入列表

                        var dto = ObjectMapper.Map<Follow, FollowDto>(follow);
                        followDtos.Add(dto);
                    }
                }
                if (followDtos.Count == 0)
                {
                    return ApiResult<List<FollowDto>>.Fail(ResultCode.Fail, "没有跟进记录");
                }
                return ApiResult<List<FollowDto>>.Success(ResultCode.Ok, followDtos);
            }
            catch (Exception ex)
            {
                logger.LogError("学员跟踪列表出错" + ex.Message);
                throw;
            }
        }
    }
}
