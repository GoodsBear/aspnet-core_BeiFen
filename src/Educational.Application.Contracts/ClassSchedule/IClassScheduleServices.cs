using Educational;
using Educational.ClassSchedule.DTO;
using Educational.ClassSchedule.Update;
using Educational.Subject;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.ClassSchedule
{

    public interface IClassScheduleServices:IApplicationService
    {
        //添加排课表
        Task<ApiResult<ClassScheduleDto>> CreateClassScheduleAsync(UpdateClassScheduleDto input);
        //查询排课表
         Task<ApiResult<ApiPaging<List<ClassScheduleDto>>>> GetListAsync([FromQuery] ClassScheduleSearchDto search);
        //反填排课表
        Task<ApiResult<ClassSchedule>> GetOneAsync(Guid id);
        //修改排课表
        Task<ApiResult> UpdateClassScheduleAsync(Guid id, ClassSchedule input);

        //批量删除排课表
        Task<ApiResult> DeletedClassAsync(List<Guid> guids);

        //冲突显示表
        Task<ApiResult<ApiPaging<List<ConflictModelDto>>>> GetConfilcListAsync([FromQuery] Seach search);
        //检查冲突-- 
        Task<ApiResult> CheckConflict(ClassScheduleDto dto);
    }
}