using Educational;
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
        Task<ApiResult<SubjectDto>> CreateSubjectAsync(UpdateSubjectDto input);
        //查询排课表
       // Task<ApiResult<ApiPaging<List<ClassScheduleDto>>>> GetListAsync([FromQuery] ClassScheduleSearchDto search);
        //反填排课表
        Task<ApiResult<ClassSchedule>> GetOneAsync(Guid id);
        //修改排课表
       // Task<ApiResult> GetOneAsync(Guid id);

        //批量删除排课表
    }
}  
/// <summary>
/// 修改字段
/// </summary>
/// <param name="input">修改字段</param>
/// <returns></returns>
//Task<ApiResult<SubjectModel>> UpdateAsync(Guid id, UpdateSubjectDto input);
///// <summary>
///// 主键删除--逻辑删除
///// </summary> 
//Task<ApiResult> SubjectDeleteAsync(List<Guid> guids);
///// <summary>
///// 科目管理下拉表
///// </summary> 
//Task<ApiResult<List<XialaSubjectDto>>> GetSubjectAsync();