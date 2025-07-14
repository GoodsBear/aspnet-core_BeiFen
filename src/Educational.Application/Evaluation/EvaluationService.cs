using Educational.Courses;
using Educational.Staffs;
using Educational.StudentsAndParends.Students;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Evaluation
{
    [ApiExplorerSettings(GroupName = "成员")]
    public class EvaluationService : ApplicationService, IEvaluationService
    {
        private readonly IRepository<EvaluationInfo, Guid> evaluationrep;
        private readonly IRepository<Student, Guid> studentrep;
        private readonly IRepository<Course, Guid> courserep;
        private readonly IRepository<StaffInfo, Guid> staffrep;

        public EvaluationService(IRepository<EvaluationInfo,Guid> evaluationrep, IRepository<Student, Guid> studentrep, IRepository<Course, Guid> courserep, IRepository<StaffInfo, Guid> staffrep)
        {
            this.evaluationrep = evaluationrep;
            this.studentrep = studentrep;
            this.courserep = courserep;
            this.staffrep = staffrep;
        }

        /// <summary>
        /// 评价教师
        /// </summary>
        /// <param name="addEvaluationDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<List<ShowEvaluationDTO>>> AddEvaluationAsync(AddEvaluationDTO addEvaluationDTO)
        {
            var addinfo=ObjectMapper.Map<AddEvaluationDTO, EvaluationInfo>(addEvaluationDTO);
            await evaluationrep.InsertAsync(addinfo);
            var showdto = ObjectMapper.Map<EvaluationInfo, ShowEvaluationDTO>(addinfo);
            return ApiResult<List<ShowEvaluationDTO>>.Success(ResultCode.Ok, new List<ShowEvaluationDTO> { showdto });
        }

        /// <summary>
        /// 学评教列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<ShowEvaluationDTO>>> ShowEvaluationAsync()
        {
            var evaluation =await evaluationrep.GetQueryableAsync();
            var dtoinfo=ObjectMapper.Map<List<EvaluationInfo>, List<ShowEvaluationDTO>>(evaluation.ToList());
            foreach (var item in dtoinfo)
            {
                item.StudentName = (await studentrep.GetAsync(item.StudentId)).Name;
                item.CourseName = (await courserep.GetAsync(item.CourseId)).CourseName;
                item.TeacherName = (await staffrep.GetAsync(item.TeacherId)).StaffName;
            }
            return ApiResult<List<ShowEvaluationDTO>>.Success(ResultCode.Ok, dtoinfo);
        }

        /// <summary>
        /// 学员登录
        /// </summary>
        /// <param name="studentLoginDTO"></param>
        /// <returns></returns>
        public async Task<ApiResult> StudentLogin(StudentLoginDTO studentLoginDTO)
        {
            var logininfo=await studentrep.FirstOrDefaultAsync(x=>x.Phone==studentLoginDTO.Phone&&studentLoginDTO.Password=="123456");
            if (logininfo == null)
            {
                return ApiResult.Fail(ResultCode.Fail, "改学员不存在！");
            }
            return ApiResult.Success(ResultCode.Ok);
        }
    }
}
