using Educational.RBAC.RoleManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StudentsAndParents.Students
{
    public interface IStudentServices
    {
        /// <summary>
        /// 新增学员
        /// </summary>
        /// <param name="createUpdateStudentDto"></param>
        /// <returns></returns>
        Task<ApiResult<StudentsDto>> AddAsync(CreateUpdateStudentDto createUpdateStudentDto);
        /// <summary>
        /// 分页查询  在学 学员列表
        /// </summary>
        /// <param name="searchStudentDto"></param>
        /// <returns></returns>
        Task<ApiResult<ApiPaging<List<StudentsDto>>>> PageLesson(SearchStudentDto searchStudentDto);

        /// <summary>
        /// 分页查询  意向 学员列表
        /// </summary>
        /// <param name="searchStudentDto"></param>
        /// <returns></returns>
        Task<ApiResult<ApiPaging<List<StudentsDto>>>> PageReady(SearchStudentDto searchStudentDto);

        /// <summary>
        /// 分页查询  结业 学员列表
        /// </summary>
        /// <param name="searchStudentDto"></param>
        /// <returns></returns>
        Task<ApiResult<ApiPaging<List<StudentsDto>>>> PageFinish(SearchStudentDto searchStudentDto);

        /// <summary>
        /// 删除学员
        /// </summary>
        Task<ApiResult> DelStudent(Guid guid);

        Task<ApiResult> DelStudentList(List<Guid> guids);
        /// <summary>
        /// 更新学员
        /// </summary>
        Task<ApiResult<StudentsDto>> UpdateStudent(CreateUpdateStudentDto createUpdateStudentDto, Guid guid);
        /// <summary>
        /// 更新学员状态
        /// </summary>
        Task<ApiResult<StudentsDto>> UpdateStudentType(Guid guid,CreateUpdateStudentDto createUpdateStudentDto);
        /// <summary>
        /// 转校
        /// </summary>
        Task<ApiResult<StudentsDto>> UpdateStudentSchool(Guid guid, Guid campusId);
        /// <summary>
        /// 修改顾问
        /// </summary>
        Task<ApiResult<StudentsDto>> UpdateStudentConsultant(Guid guid, Guid consultant);
    }
}
