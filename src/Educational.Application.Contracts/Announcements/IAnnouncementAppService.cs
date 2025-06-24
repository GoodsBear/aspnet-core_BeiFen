using Educational.Dto.Announcements;
using Educational.Dto.Positions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.Announcements
{
    public interface IAnnouncementAppService:IApplicationService
    {
        Task<ApiResult<AnnouncementDto>> CreatePosition(CreateUpdateAnnouncement createAnnouncement);
        Task<ApiResult<AnnouncementDto>> UpdatePosition(Guid id, CreateUpdateAnnouncement createAnnouncement);
        Task<ApiResult<ApiPaging<List<AnnouncementDto>>>> GetPositionList([FromQuery] AnnouncementSearchDto searchDto);
        Task<ApiResult> DeletePosition(Guid id);
        Task<ApiResult> BatchDelete(List<Guid> ids);
    }
}
