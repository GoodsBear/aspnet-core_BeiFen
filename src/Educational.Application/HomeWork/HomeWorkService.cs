using Educational.Classgrade;
using Educational.Staffs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.HomeWork
{
    [ApiExplorerSettings(GroupName = "成员")]
    public class HomeWorkService : ApplicationService, IHomeWorkService
    {
        private readonly IRepository<HomeWorkInfo, Guid> homeworkrep;
        private readonly IRepository<ClassInfo, Guid> classrep;
        private readonly IRepository<StaffInfo, Guid> staffrep;

        public HomeWorkService(IRepository<HomeWorkInfo,Guid> homeworkrep,IRepository<ClassInfo,Guid> classrep,IRepository<StaffInfo,Guid> staffrep)
        {
            this.homeworkrep = homeworkrep;
            this.classrep = classrep;
            this.staffrep = staffrep;
        }
        /// <summary>
        /// 布置作业
        /// </summary>
        /// <param name="arrangeHomeWorkDTO"></param>
        /// <returns></returns>
        public async Task<ApiResult> ArrangeHomeWork(ArrangeHomeWorkDTO arrangeHomeWorkDTO)
        {
            //判断是否存在
            var homework = homeworkrep.FirstOrDefaultAsync(x => x.WorkTitle == arrangeHomeWorkDTO.WorkTitle);
            if (homework != null)
            {
                return ApiResult.Fail(ResultCode.Fail, "该课后作业已存在");
            }
            //新增作业
            var addinfo=ObjectMapper.Map<ArrangeHomeWorkDTO,HomeWorkInfo>(arrangeHomeWorkDTO);
            await homeworkrep.InsertAsync(addinfo);
            return ApiResult.Success(ResultCode.Ok);
        }

        /// <summary>
        /// 课后作业列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<ShowWorkDTO>>> GetHomeWorkList()
        {
            var homeworklist = await homeworkrep.GetQueryableAsync();
            var dtoinfo = ObjectMapper.Map<List<HomeWorkInfo>, List<ShowWorkDTO>>(homeworklist.ToList());
            foreach (var item in dtoinfo)
            {
                item.ClassName=(await classrep.GetAsync(item.ClassId)).ClassName;
                item.IssuerName=(await staffrep.GetAsync(item.Issuer)).StaffName;
            }
            return ApiResult<List<ShowWorkDTO>>.Success(ResultCode.Ok, dtoinfo);
        }
    }
}
