using Educational.ClassSchedule.DTO;
using Educational.ClassSchedule.Update;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.ClassSchedule
{
    public interface IConflictModelServices : IApplicationService
    {
        //查询冲突表 
        Task<ApiResult<ApiPaging<List<ConflictModelDto>>>> GetConfilcListAsync([FromQuery] ConflictSearchDto search);
        //添加冲突表
        Task<ApiResult<ConflictModelDto>> CreateClassScheduleAsync(List<UpdateConflictModel> input);
    }
}
