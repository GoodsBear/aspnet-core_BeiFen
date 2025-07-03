using Educational.Classgrade;
using Educational.Enums;
using Educational.Organization;
using Educational.Staffs;
using Educational.StudentsAndParends.Parents;
using Educational.StudentsAndParends.Students;
using Educational.StudentsAndParents.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Educational.StudentsAndParents.StudentServices
{
    [ApiExplorerSettings(GroupName = "学员")]
    public class StudentServices : ApplicationService, IStudentServices
    {
        private readonly IRepository<Student, Guid> repository;
        private readonly IRepository<Parent, Guid> parentRepository;
        private readonly IRepository<Grade, Guid> gardrepository;
        private readonly IRepository<StaffInfo, Guid> staffrepository;
        private readonly IRepository<OrganizationModel> organizationrepository;
        private readonly ILogger<StudentServices> logger;

		public StudentServices(IRepository<Student, Guid> repository, IRepository<Educational.Classgrade.Grade, Guid> gardrepository, IRepository<StaffInfo, Guid> staffrepository,
			IRepository<OrganizationModel> organizationrepository,
			ILogger<StudentServices> logger, IRepository<Parent, Guid> parentRepository)
		{
			this.repository = repository;
			this.gardrepository = gardrepository;
			this.staffrepository = staffrepository;
			this.organizationrepository = organizationrepository;
			this.logger = logger;
			this.parentRepository = parentRepository;
		}

		/// <summary>
		/// 新增学员
		/// </summary>
		[HttpPost]
        [UnitOfWork]
        public async Task<ApiResult<StudentsDto>> AddAsync(CreateUpdateStudentDto createUpdateStudentDto)
        {
            try
            {
                // 检查身份证号是否已存在
                var exist = await repository.GetListAsync(d => d.IdCard == createUpdateStudentDto.IdCard);
                if (exist.Count != 0)
                {

                    return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "该身份证号已存在，请勿重复添加！");
                }   

                var entity = ObjectMapper.Map<CreateUpdateStudentDto, Student>(createUpdateStudentDto);

                // 根据身份证号计算年龄
                entity.Age = CalculateAge(entity.IdCard);

                var res = await repository.InsertAsync(entity);

                if (res != null)
                {
                    var dto = ObjectMapper.Map<Student, StudentsDto>(res);
                    return ApiResult<StudentsDto>.Success(ResultCode.Ok, dto);
                }
                else
                {



                    var parent = await parentRepository.GetListAsync(d => d.Phone == createUpdateStudentDto.Phone);
                    // 检查手机号是否已存在
                    if (parent.Count == 0)
                    {
                        Parent par = new Parent();
                        par.PardentName = createUpdateStudentDto.ParentName;
                        par.Phone = createUpdateStudentDto.Phone;
                        par.CreationTime = DateTime.Now;
                        par.Status = true;
                        await parentRepository.InsertAsync(par);
                    }
                }

                var student = ObjectMapper.Map<CreateUpdateStudentDto, Student>(createUpdateStudentDto);
                var result = await repository.InsertAsync(student);
                return ApiResult<StudentsDto>.Success(ResultCode.Ok, ObjectMapper.Map<Student, StudentsDto>(entity));
            }
            catch (Exception ex)
            {
                logger.LogError("学员添加出错: " + ex.Message);
                return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "添加失败！");
            }
        }

        /// <summary>
        /// 分页查询结业学员
        /// </summary>
        [HttpGet]
        public async Task<ApiResult<ApiPaging<List<StudentsDto>>>> PageFinish([FromQuery] SearchStudentDto searchStudentDto)
        {
            try
            {
                //获取所有学员数据
                var list = await repository.GetQueryableAsync();

                list = list.Where(d => d.StudentType == Educational.Enums.StudentEnum.结业学员);
                list = list.WhereIf(!string.IsNullOrEmpty(searchStudentDto.StudentName), d => d.Name.Contains(searchStudentDto.StudentName));
                if (searchStudentDto.GradeId != null)
                {
                    list = list.Where(d => d.GradeId == searchStudentDto.GradeId);
                }
                if (searchStudentDto.OrgaizationId != null)
                {
                    list = list.Where(d => d.CampusId == searchStudentDto.OrgaizationId);
                }
                //获取所有校区数据
                var school = await organizationrepository.GetQueryableAsync();
                //获取所有年级数据
                var gardes = await gardrepository.GetQueryableAsync();
                //获取所有顾问数据
                var staffs = await staffrepository.GetQueryableAsync();

                var query = from a in list
                            join b in gardes on a.GradeId equals b.Id
                            join c in staffs on a.Consultant equals c.Id
                            join d in school on a.CampusId equals d.Id
                            select new StudentsDto
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Phone = a.Phone,
                                CampusId = a.CampusId,
                                CampusName = d.Name,
                                ParentName = a.ParentName,
                                Relation = a.Relation,
                                Sex = a.Sex,
                                EnrollTime = a.EnrollTime,
                                GradeId = a.GradeId,
                                GradeName = b.GradeName,
                                Birthday = a.Birthday,
                                IdCard = a.IdCard,
                                Source = a.Source,
                                Remark = a.Remark,
                                StudentType = a.StudentType,
                                Consultant = a.Consultant,
                                StaffName = c.StaffName,
                                LessonNums = a.LessonNums,
                                Age = a.Age
                            };

                var page = query.PageResult(searchStudentDto.PageIndex, searchStudentDto.PageSize);

                var paging = new ApiPaging<List<StudentsDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / page.PageSize),
                    Data = page.Queryable.ToList()
                };
                return ApiResult<ApiPaging<List<StudentsDto>>>.Success(ResultCode.Ok, paging);
            }
            catch (Exception ex)
            {
                logger.LogError("结业学员分页查询出错: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 分页查询在学学员
        /// </summary>
        [HttpGet]
        public async Task<ApiResult<ApiPaging<List<StudentsDto>>>> PageLesson([FromQuery] SearchStudentDto searchStudentDto)
        {
            try
            {
                var list = await repository.GetQueryableAsync();
                list = list.Where(d => d.StudentType == Educational.Enums.StudentEnum.在线学员);
                list = list.WhereIf(!string.IsNullOrEmpty(searchStudentDto.StudentName), d => d.Name.Contains(searchStudentDto.StudentName));
                if (searchStudentDto.GradeId != null)
                {
                    list = list.Where(d => d.GradeId == searchStudentDto.GradeId);
                }
                if (searchStudentDto.OrgaizationId != null)
                {
                    list = list.Where(d => d.CampusId == searchStudentDto.OrgaizationId);
                }

                //获取所有校区数据
                var school = await organizationrepository.GetQueryableAsync();
                //获取所有年级数据
                var gardes = await gardrepository.GetQueryableAsync();
                //获取所有顾问数据
                var staffs = await staffrepository.GetQueryableAsync();

                var query = from a in list
                            join b in gardes on a.GradeId equals b.Id
                            join c in staffs on a.Consultant equals c.Id
                            join d in school on a.CampusId equals d.Id
                            select new StudentsDto
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Phone = a.Phone,
                                CampusId = a.CampusId,
                                CampusName = d.Name,
                                ParentName = a.ParentName,
                                Relation = a.Relation,
                                Sex = a.Sex,
                                EnrollTime = a.EnrollTime,
                                GradeId = a.GradeId,
                                GradeName = b.GradeName,
                                Birthday = a.Birthday,
                                IdCard = a.IdCard,
                                Source = a.Source,
                                Remark = a.Remark,
                                StudentType = a.StudentType,
                                Consultant = a.Consultant,
                                StaffName = c.StaffName,
                                LessonNums = a.LessonNums,
                                Age = a.Age
                            };

                var page = query.PageResult(searchStudentDto.PageIndex, searchStudentDto.PageSize);

                var paging = new ApiPaging<List<StudentsDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / page.PageSize),
                    Data = page.Queryable.ToList()
                };
                return ApiResult<ApiPaging<List<StudentsDto>>>.Success(ResultCode.Ok, paging);
            }
            catch (Exception ex)
            {
                logger.LogError("在学学员分页查询出错: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 分页查询意向学员
        /// </summary>
        [HttpGet]
        public async Task<ApiResult<ApiPaging<List<StudentsDto>>>> PageReady([FromQuery] SearchStudentDto searchStudentDto)
        {
            try
            {
                var list = await repository.GetQueryableAsync();
                list = list.Where(d => d.StudentType == Educational.Enums.StudentEnum.意向学员);
                list = list.WhereIf(!string.IsNullOrEmpty(searchStudentDto.StudentName), d => d.Name.Contains(searchStudentDto.StudentName));
                if(searchStudentDto.GradeId != null)
                {
                    list = list.Where(d => d.GradeId == searchStudentDto.GradeId);
                }
                if (searchStudentDto.OrgaizationId != null)
                {
                    list = list.Where(d => d.CampusId == searchStudentDto.OrgaizationId);
                }
                //获取所有校区数据
                var school = await organizationrepository.GetQueryableAsync();
                //获取所有年级数据
                var gardes = await gardrepository.GetQueryableAsync();
                //获取所有顾问数据
                var staffs = await staffrepository.GetQueryableAsync();

                var query = from a in list
                            join b in gardes on a.GradeId equals b.Id
                            join c in staffs on a.Consultant equals c.Id
                            join d in school on a.CampusId equals d.Id
                            select new StudentsDto
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Phone = a.Phone,
                                CampusId = a.CampusId,
                                CampusName = d.Name,
                                ParentName = a.ParentName,
                                Relation = a.Relation,
                                Sex = a.Sex,
                                EnrollTime = a.EnrollTime,
                                GradeId = a.GradeId,
                                GradeName = b.GradeName,
                                Birthday = a.Birthday,
                                IdCard = a.IdCard,
                                Source = a.Source,
                                Remark = a.Remark,
                                StudentType = a.StudentType,
                                Consultant = a.Consultant,
                                StaffName = c.StaffName,
                                LessonNums = a.LessonNums,
                                Age = a.Age
                            };

                var page = query.PageResult(searchStudentDto.PageIndex, searchStudentDto.PageSize);

                var paging = new ApiPaging<List<StudentsDto>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / page.PageSize),
                    Data = page.Queryable.ToList()
                };
                return ApiResult<ApiPaging<List<StudentsDto>>>.Success(ResultCode.Ok, paging);
            }
            catch (Exception ex)
            {
                logger.LogError("意向学员分页查询出错: " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 删除学员
        /// </summary>
        [HttpDelete]
        public async Task<ApiResult> DelStudent(Guid guid)
        {
            try
            {
                await repository.DeleteAsync(guid);
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("学员删除失败: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 更新学员
        /// </summary>
        [HttpPut]
        public async Task<ApiResult<StudentsDto>> UpdateStudent(CreateUpdateStudentDto createUpdateStudentDto, Guid guid)
        {
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var entity = await repository.GetAsync(guid);
                    if (entity == null)
                    {
                        return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "学员不存在，请检查！");
                    }

                    var data = ObjectMapper.Map(createUpdateStudentDto, entity);
                    entity.Age = CalculateAge(entity.IdCard);
                    var res = await repository.UpdateAsync(data);

                    if (res != null)
                    {
                        var dto = ObjectMapper.Map<Student, StudentsDto>(res);
                        tran.Complete();
                        return ApiResult<StudentsDto>.Success(ResultCode.Ok, dto);
                    }
                    else
                    {
                        return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "学员修改失败");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("学员修改出错: " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 将当前学员状态转变为在线学员、意向学员或结业学员
        /// </summary>
        [HttpPut]
        public async Task<ApiResult> UpdateStudentType(List<Guid> guids, StudentEnum studentEnum)
        {
            try
            {

                foreach (var item in guids)
                {
                    var list = await repository.GetAsync(d => d.Id == item);
                    if (list == null)
                    {
                        return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "学员不存在，请检查！");
                    }
                    if (list.StudentType == studentEnum)
                    {
                        return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "该学员已是此状态，无需修改！");
                    }
                    list.StudentType = studentEnum;
                    var res = await repository.UpdateAsync(list);
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("学员状态更新出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 转校
        /// </summary>
        public async Task<ApiResult> UpdateStudentSchool(List<Guid> guids, Guid campusId)
        {
            try
            {

                foreach (var item in guids)
                {
                    var list = await repository.GetAsync(d => d.Id == item);
                    if (list == null)
                    {
                        return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "学员不存在，请检查！");
                    }
                    if (list.CampusId == campusId)
                    {
                        return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "该学员已在此校区，无需转校！");
                    }
                    list.CampusId = campusId;
                    var res = await repository.UpdateAsync(list);
                }
                    return ApiResult.Success(ResultCode.Ok);
                }
            catch (Exception ex)
            {
                logger.LogError("学员转校出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 修改顾问
        /// </summary>
        [HttpPut]
        public async Task<ApiResult> UpdateStudentConsultant(List<Guid> guids, Guid consultant)
        {
            try
            {

                foreach (var item in guids)
                {
                    var list = await repository.GetAsync(d => d.Id == item);
                    if (list == null)
                    {
                        return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "学员不存在，请检查！");
                    }
                    if (list.Consultant == consultant)
                    {
                        return ApiResult<StudentsDto>.Fail(ResultCode.Fail, "该学员已是此顾问，无需修改！");
                    }
                    list.Consultant = consultant;
                    var res = await repository.UpdateAsync(list);
                }
                    return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("顾问修改出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DelStudentList(List<Guid> guids)
        {
            try
            {
                foreach (var item in guids)
                {
                    var entity = await repository.GetAsync(item);

                     await repository.DeleteAsync(item);
                }

                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("批量删除操作出错" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 根据身份证号计算年龄
        /// </summary>
        private int CalculateAge(string idCard)
        {
            if (string.IsNullOrEmpty(idCard) || idCard.Length != 18)
            {
                return 0;
            }

            try
            {
                // 截取身份证中的出生年月日
                string birthYear = idCard.Substring(6, 4);
                string birthMonth = idCard.Substring(10, 2);
                string birthDay = idCard.Substring(12, 2);

                // 转换为日期
                DateTime birthDate = new DateTime(int.Parse(birthYear), int.Parse(birthMonth), int.Parse(birthDay));
                DateTime nowDate = DateTime.Now;

                // 计算年龄
                int age = nowDate.Year - birthDate.Year;
                // 如果今年的生日还没过，年龄减1
                if (birthDate.Date > nowDate.Date.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
            catch
            {
                return 0;
            }
        }
    }
}
