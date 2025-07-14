using Educational.Enums;
using Educational.Staffs;
using Educational.StudentsAndParends.Students;
using Educational.StudentsAndParends.Students.Follow;
using Educational.StudentsAndParents.Follow;
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
        private readonly IRepository<Educational.StudentsAndParends.Students.Follow.Follow,Guid> followrepository;
        private readonly IRepository<StudentFollowRelation,Guid> studentfollowrepository;
        private readonly IRepository<Student, Guid> studentrepository;
        private readonly IRepository<StaffInfo, Guid>  staffinforepository;
        private readonly ILogger<FollowServices> logger;

        public FollowServices(IRepository<StudentsAndParends.Students.Follow.Follow, Guid> followrepository, IRepository<StudentFollowRelation, Guid> studentfollowrepository, ILogger<FollowServices> logger, IRepository<Student, Guid> studentrepository, IRepository<StaffInfo, Guid> staffinforepository)
        {
            this.followrepository = followrepository;
            this.studentfollowrepository = studentfollowrepository;
            this.logger = logger;
            this.studentrepository = studentrepository;
            this.staffinforepository = staffinforepository;
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
                    var data = ObjectMapper.Map<CreateUpdateFollowDto, Educational.StudentsAndParends.Students.Follow.Follow>(createUpdateFollowDto);

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
                        return  ApiResult<FollowDto>.Success(ResultCode.Ok,ObjectMapper.Map<Educational.StudentsAndParends.Students.Follow.Follow, FollowDto>(data));
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

                        var dto = ObjectMapper.Map<Educational.StudentsAndParends.Students.Follow.Follow, FollowDto>(follow);
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
        /// <summary>
        /// 获取跟进列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<ApiPaging<List<FollowDto>>>> GetFollowRecordList([FromQuery] FollowSearchDto searchDto)
        {
            try
            {
                var studentfollowList = await studentfollowrepository.GetQueryableAsync();
                var followList = await followrepository.GetQueryableAsync();
                var staffinfoList = await staffinforepository.GetQueryableAsync();
                var studentList = await studentrepository.GetQueryableAsync();
                var list = from studentfollow in studentfollowList
                           join follow in followList on studentfollow.FollowId equals follow.Id
                           join student in studentList on studentfollow.StudentId equals student.Id
                           join staffinfo in staffinfoList on student.Consultant equals staffinfo.Id
                           select new FollowDto
                           {
                               Id = follow.Id,
                               StudentId = student.Id,
                               StudentName = student.Name,
                               Consultant = student.Consultant,
                               ConsultantName = staffinfo.StaffName,
                               FollowId = follow.Id,
                               RecordDate = follow.RecordDate,
                               FollowStageEnum = follow.FollowStageEnum,
                               GetInTouchEnum = follow.GetInTouchEnum,
                               TouchTIme = follow.TouchTIme,
                               NextTouchTime = follow.NextTouchTime,
                               TouchWay = follow.TouchWay,
                               FollowDesc = follow.FollowDesc
                           };
                // 条件过滤
                if (!string.IsNullOrEmpty(searchDto.Name))
                {
                    
                    list = list.Where(x => x.StudentName.Contains(searchDto.Name));
                }
                if (searchDto.FollowStageEnum != null)
                {
                    list = list.Where(x => x.FollowStageEnum == (FollowStageEnums)searchDto.FollowStageEnum);
                }
                if (searchDto.Consultant != null)
                {
                    list = list.Where(x => x.Consultant == searchDto.Consultant);
                }
                var paging = list.PageResult(searchDto.PageIndex, searchDto.PageSize);
                var result = new ApiPaging<List<FollowDto>>
                {
                    TotleCount = paging.RowCount,
                    TotlePage = (int)Math.Ceiling(paging.RowCount * 1.0 / searchDto.PageSize),
                    Data = paging.Queryable.ToList()
                };
                
                return ApiResult<ApiPaging<List<FollowDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("学员跟进列表出错" + ex.Message);
                throw;
            }
        }
    }
}
