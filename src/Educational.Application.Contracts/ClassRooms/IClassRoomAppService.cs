using Educational.Classgrade;
using Educational.Dto.ClassRooms;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.ClassRooms
{
    public interface  IClassRoomAppService : IApplicationService
    {
        Task<ApiResult<ClassRoomDto>> CreateClassRoom(CreateUpdateClassRoomDto createClassRoomDto);
        Task<ApiResult<ClassRoomDto>> UpdateClassRoom(Guid id, CreateUpdateClassRoomDto createClassRoomDto);
        Task<ApiResult<ApiPaging<List<ClassRoomDto>>>> GetClassRoomList([FromQuery] ClassRoomSeachDto searchDto);
        Task<ApiResult> BatchDelete(List<Guid> ids);

    }
}
