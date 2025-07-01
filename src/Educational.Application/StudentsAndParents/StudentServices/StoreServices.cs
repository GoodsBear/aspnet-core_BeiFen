using Educational.StudentsAndParends.Students;
using Educational.StudentsAndParends.Students.Follow;
using Educational.StudentsAndParends.Students.Store;
using Educational.StudentsAndParents.Stores;
using Educational.StudentsAndParents.StudentFollow;
using Educational.StudentsAndParents.StudentStore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Educational.StudentsAndParents.StudentServices
{
    [ApiExplorerSettings(GroupName = "积分")]
    public class StoreServices : ApplicationService, IStoreServices
    {
        private readonly IRepository<Store> storerepository;
        private readonly IRepository<StudentStoreRelation> relationrepository;
        private readonly IRepository<Student> studentrepository;
        private readonly ILogger<StoreServices> logger;

        public StoreServices(IRepository<Store> storerepository, IRepository<StudentStoreRelation> relationrepository, IRepository<Student> studentrepository, ILogger<StoreServices> logger)
        {
            this.storerepository = storerepository;
            this.relationrepository = relationrepository;
            this.studentrepository = studentrepository;
            this.logger = logger;
        }

        /// <summary>
        /// 积分调整
        /// </summary>
        /// <param name="createUpdatestoreDto">调整内容</param>
        /// <param name="guid">学员Id</param>
        /// <param name="AddOrRecude">(1:添加 2:扣除)</param>
        /// <returns></returns>
        public async Task<ApiResult<StoreDto>> AddAsync(CreateUpdateStoreDto createUpdatestoreDto, Guid guid, int AddOrRecude)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (AddOrRecude == 1)
                    {
                        createUpdatestoreDto.ExcessStores = createUpdatestoreDto.ExcessStores + createUpdatestoreDto.ChangeStores;
                    }
                    else
                    {
                        if (createUpdatestoreDto.ExcessStores == 0)
                        {
                            return ApiResult<StoreDto>.Fail(ResultCode.Fail, "当前积分为0，不能在继续减少了！");
                        }
                        createUpdatestoreDto.ExcessStores = createUpdatestoreDto.ExcessStores - createUpdatestoreDto.ChangeStores;
                        if (createUpdatestoreDto.ExcessStores < 0)
                        {
                            createUpdatestoreDto.ExcessStores = 0;
                        }
                    }

                    var data = ObjectMapper.Map<CreateUpdateStoreDto, Store>(createUpdatestoreDto);

                    await storerepository.InsertAsync(data);

                    MeddleStoreDto meddleStoreDto = new MeddleStoreDto()
                    {
                        StudentId = guid,
                        StoreId = data.Id
                    };
                    var result = ObjectMapper.Map<MeddleStoreDto, StudentStoreRelation>(meddleStoreDto);
                    var query = await relationrepository.InsertAsync(result);

                    if (query != null)
                    {
                        return ApiResult<StoreDto>.Success(ResultCode.Ok, ObjectMapper.Map<Store, StoreDto>(data));
                    }
                    else
                    {
                        return ApiResult<StoreDto>.Fail(ResultCode.Fail, "添加跟进失败");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("积分调整出错" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 学生对应积分列表
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<ApiPaging<List<StoreDto>>>> PageAsync(Guid guid, [FromQuery] SearchDto searchDto)
        {
            try
            {
                var list = await relationrepository.GetListAsync(d => d.StudentId == guid);
                if (list == null || list.Count == 0)
                {
                    return ApiResult<ApiPaging<List<StoreDto>>>.Fail(ResultCode.Fail, "没有查询到对应的积分记录！");
                }

                var storeDto = new List<StoreDto>();

                foreach (var item in list)
                {
                    var store = await storerepository.GetAsync(d => d.Id == item.StoreId);
                    if (store != null)
                    {
                        // 时间筛选
                        if (searchDto.CreateTime != null && store.CreationTime < searchDto.CreateTime)
                        {
                            continue;
                        }
                        if (searchDto.EndTime != null && store.CreationTime > searchDto.EndTime)
                        {
                            continue;
                        }
                        storeDto.Add(ObjectMapper.Map<Store, StoreDto>(store));
                    }
                }

                if (storeDto.Count == 0)
                {
                    return ApiResult<ApiPaging<List<StoreDto>>>.Fail(ResultCode.Fail, "没有查询到对应的积分记录！");
                }

                // 分页
                int totalCount = storeDto.Count;
                int pageIndex = searchDto.PageIndex > 0 ? searchDto.PageIndex : 1;
                int pageSize = searchDto.PageSize > 0 ? searchDto.PageSize : 10;
                int totalPage = (int)Math.Ceiling(totalCount * 1.0 / pageSize);

                var pageData = storeDto
                    .OrderByDescending(x => x.ChangeCause) // 如需按时间排序可改为 CreationTime
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var paging = new ApiPaging<List<StoreDto>>
                {
                    TotleCount = totalCount,
                    TotlePage = totalPage,
                    Data = pageData
                };

                return ApiResult<ApiPaging<List<StoreDto>>>.Success(ResultCode.Ok, paging);
            }
            catch (Exception ex)
            {
                logger.LogError("积分调整列表出错" + ex.Message);
                throw;
            }
        }
    }
}
