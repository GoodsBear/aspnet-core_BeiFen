using Educational.Classgrade;
using Educational.Dto.ClassRooms;
using Educational.Dto.Positions;
using Educational.Organization;
using Educational.Positions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.ClassRooms
{
    [ApiExplorerSettings(GroupName = "教室")]
    public class ClassRoomAppService : ApplicationService, IClassRoomAppService
    {
        IRepository<ClassRoom, Guid> classRoomRep;
        ILogger<ClassRoomAppService> logger;

        public ClassRoomAppService(IRepository<ClassRoom, Guid> classRoomRep, ILogger<ClassRoomAppService> logger)
        {
            this.classRoomRep = classRoomRep;
            this.logger = logger;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">教室id数组</param>
        /// <returns>返回受影响行数</returns>
        [HttpDelete]
        public async Task<ApiResult> BatchDelete(List<Guid> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var classroom = await classRoomRep.GetAsync(item);
                    if (classroom == null)
                    {
                        return ApiResult.Fail(ResultCode.Fail, "教室不存在！");
                    }
                    await classRoomRep.DeleteAsync(classroom);

                }
                return ApiResult.Success(ResultCode.Ok);

            }
            catch (Exception ex)
            {
                logger.LogError("教室批量删除出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 创建教室
        /// </summary>
        /// <param name="createClassRoomDto">教室dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult<ClassRoomDto>> CreateClassRoom(CreateUpdateClassRoomDto createClassRoomDto)
        {
            try
            {
                var classRoom = ObjectMapper.Map<CreateUpdateClassRoomDto, ClassRoom>(createClassRoomDto);
                var result = await classRoomRep.InsertAsync(classRoom);
                var classRoomDto = ObjectMapper.Map<ClassRoom, ClassRoomDto>(result);
                return ApiResult<ClassRoomDto>.Success(ResultCode.Ok, classRoomDto);
            }
            catch (Exception ex)
            {
                logger.LogError("教室添加出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获取教室列表下拉框
        /// </summary>
        /// <returns>返回教室列表下拉框</returns>
        public async Task<ApiResult<List<ClassRoomSelectDto>>> GetClassRoomAsync()
        {
            try
            {
                var queryable = await classRoomRep.GetListAsync();
                var results = ObjectMapper.Map<List<ClassRoom>, List<ClassRoomSelectDto>>(queryable);
                return ApiResult<List<ClassRoomSelectDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "教室下拉框获取失败");
                throw;
            }
        }

        /// <summary>
        /// 获取教室列表
        /// </summary>
        /// <param name="searchDto">查询dto</param>
        /// <returns>返回教室列表</returns>
        public async Task<ApiResult<ApiPaging<List<ClassRoomDto>>>> GetClassRoomList([FromQuery] ClassRoomSeachDto searchDto)
        {
            try
            {
                var list = await classRoomRep.GetQueryableAsync();
                list = list.WhereIf(!string.IsNullOrEmpty(searchDto.ClassRoomName), x => x.ClassRoomName.Contains(searchDto.ClassRoomName));
                var page = list.PageResult(searchDto.PageIndex, searchDto.PageSize);
                var classRoomDto = ObjectMapper.Map<List<ClassRoom>, List<ClassRoomDto>>(page.Queryable.ToList());
                var result = new ApiPaging<List<ClassRoomDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / searchDto.PageSize),
                    Data = classRoomDto
                };
                return ApiResult<ApiPaging<List<ClassRoomDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("教室列表报错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 修改教室
        /// </summary>
        /// <param name="id">根据id查询数据</param>
        /// <param name="createClassRoomDto">教室修改dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult<ClassRoomDto>> UpdateClassRoom(Guid id, CreateUpdateClassRoomDto createClassRoomDto)
        {
            try
            {
                var classRoom = await classRoomRep.GetAsync(id);
                if (classRoom == null)
                {
                    return ApiResult<ClassRoomDto>.Fail(ResultCode.Fail, "教室不存在！");
                }
                var result = ObjectMapper.Map(createClassRoomDto, classRoom);
                await classRoomRep.UpdateAsync(result);
                return ApiResult<ClassRoomDto>.Success(ResultCode.Ok, ObjectMapper.Map<ClassRoom, ClassRoomDto>(result));
            }
            catch (Exception ex)
            {
                logger.LogError("教室修改出错！" + ex.Message);
                throw;
            }
        }

    }
}
