using Educational.Enums;
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
        /// 获取所有学员下拉框
        /// </summary>
        Task<ApiResult<List<StudentSelectDto>>> GetStudentAsync();
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
        /// 将当前学员状态转变为在线学员、意向学员或结业学员
        /// </summary>
        Task<ApiResult> UpdateStudentType(List<Guid> guids,StudentEnum studentEnum);
        /// <summary>
        /// 转校
        /// </summary>
        Task<ApiResult> UpdateStudentSchool(List<Guid> guids, Guid campusId);
        /// <summary>
        /// 修改顾问
        /// </summary>
        Task<ApiResult> UpdateStudentConsultant(List<Guid> guids, Guid consultant);
    }
}
