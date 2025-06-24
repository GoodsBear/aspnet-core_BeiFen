using Educational.Dto.Announcements;
using Educational.Dto.Positions;
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
using Volo.Abp.ObjectMapping;

namespace Educational.Announcements
{
    public class AnnouncementAppService : ApplicationService, IAnnouncementAppService
    {
        ILogger<AnnouncementAppService> logger;
        IRepository<Announcement, Guid> announcementRep;

        public AnnouncementAppService(ILogger<AnnouncementAppService> logger, IRepository<Announcement, Guid> announcementRep)
        {
            this.logger = logger;
            this.announcementRep = announcementRep;
        }
        /// <summary>
        /// 创建公告
        /// </summary>
        /// <param name="createAnnouncement">创建公告dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult<AnnouncementDto>> CreatePosition(CreateUpdateAnnouncement createAnnouncement)
        {
            try
            {
                // 创建公告
                var announcement = ObjectMapper.Map<CreateUpdateAnnouncement, Announcement>(createAnnouncement);
                // 插入数据库
                var result = await announcementRep.InsertAsync(announcement);
                //映射
                var announcementDto = ObjectMapper.Map<Announcement, AnnouncementDto>(result);
                //返回
                return ApiResult<AnnouncementDto>.Success(ResultCode.Ok, announcementDto);
            }
            catch (Exception ex)
            {
                logger.LogError("公告添加出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>

        public async Task<ApiResult<ApiPaging<List<AnnouncementDto>>>> GetPositionList([FromQuery] AnnouncementSearchDto searchDto)
        {
            try
            {
                // 获取公告列表
                var list = await announcementRep.GetQueryableAsync();
                // 公告标题查询
                list = list.WhereIf(!string.IsNullOrEmpty(searchDto.Title), x => x.Title.Contains(searchDto.Title));
                // 使用ABP自带分页方法
                var page = list.PageResult(searchDto.PageIndex, searchDto.PageSize);
                // 映射
                var positionDto = ObjectMapper.Map<List<Announcement>, List<AnnouncementDto>>(page.Queryable.ToList());
                var result = new ApiPaging<List<AnnouncementDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / searchDto.PageSize),
                    Data = positionDto
                };
                return ApiResult<ApiPaging<List<AnnouncementDto>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex )
            {
                logger.LogError("公告显示出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 公告修改
        /// </summary>
        /// <param name="id">根据id查询值</param>
        /// <param name="createAnnouncement">修改dto</param>
        /// <returns>返回受影响行数</returns>
        public async Task<ApiResult<AnnouncementDto>> UpdatePosition(Guid id, CreateUpdateAnnouncement createAnnouncement)
        {
            try
            {
                var announcement = await announcementRep.GetAsync(id);

                if (announcement == null)
                {
                    return ApiResult<AnnouncementDto>.Fail(ResultCode.Fail, "职位不存在！");
                }
                var result = ObjectMapper.Map(createAnnouncement, announcement);
                await announcementRep.UpdateAsync(result);
                return ApiResult<AnnouncementDto>.Success(ResultCode.Ok, ObjectMapper.Map<Announcement, AnnouncementDto>(result));
            }
            catch (Exception ex)
            {
                logger.LogError("公告修改出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="id">根据id查询值</param>
        /// <returns>返回受影响行数</returns>
    
        public async Task<ApiResult> DeletePosition(Guid id)
        {
            try
            {
                var announcement = await announcementRep.GetAsync(id);
                if (announcement == null)
                {
                    return ApiResult.Fail(ResultCode.Fail, "职位不存在！");
                }
                await announcementRep.DeleteAsync(announcement);
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("公告删除出错！" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">批删数组</param>
        /// <returns>返回受影响行数</returns>

        public async Task<ApiResult> BatchDelete(List<Guid> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var announcement = await announcementRep.GetAsync(item);
                    if (announcement == null)
                    {
                        return ApiResult.Fail(ResultCode.Fail, "公告不存在！");
                    }
                    await announcementRep.DeleteAsync(announcement);
                   
                }
                return ApiResult.Success(ResultCode.Ok);

            }
            catch (Exception ex)
            {
                logger.LogError("公告批量删除出错！" + ex.Message);
                throw;
            }
        }
    }
}
