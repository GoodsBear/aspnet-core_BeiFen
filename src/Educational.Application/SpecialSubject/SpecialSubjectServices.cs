using Educational.Organization;
using Educational.Staffs;
using Educational.Subject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Educational.SpecialSubject
{
    /// <summary>
    /// 专题--
    /// </summary>
    [ApiExplorerSettings(GroupName = "专题")]
    public class SpecialSubjectServices : ApplicationService, ISpecialSubjectServices
    {
        private readonly IRepository<SpecialSubjectModel, Guid> _specialSubjectRepository;
        private readonly IRepository<CategoryModel, Guid> _categoryRepository;
        private readonly IRepository<StaffInfo, Guid> _staffInfoRepository;
        ILogger<SpecialSubjectServices> logger;

        public SpecialSubjectServices(
            IRepository<SpecialSubjectModel, Guid> organizationRepository,
            IRepository<CategoryModel, Guid> organizationLevelRepository,
            ILogger<SpecialSubjectServices> logger,
            IRepository<StaffInfo, Guid> staffInfoRepository)
        {
            _specialSubjectRepository = organizationRepository;
            _categoryRepository = organizationLevelRepository;
            this.logger = logger;
            _staffInfoRepository = staffInfoRepository;
        }
        //IRepository<SpecialSubjectModel, Guid> specialSubjectRepository;
        //IRepository<CategoryModel, Guid> categoryRepository;
        //ILogger<SpecialSubjectServices> logger;
        //public SpecialSubjectServices(
        //IRepository<SpecialSubjectModel, Guid> specialSubjectRepository,
        //IRepository<CategoryModel, Guid> categoryRepository,
        //ILogger<SpecialSubjectServices> logger)
        //{
        //    specialSubjectRepository = specialSubjectRepository;
        //    categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        //    // 其他空检查...
        //    this.logger = logger;
        //}
        /// <summary>
        /// 专题添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<SpecialSubjectDto>> CreateAsync(UpdateSpecialSubjectDto input)
        {
            try
            {
                // 验证专题名是否已存在
                var existingOrg = await _specialSubjectRepository.FirstOrDefaultAsync(x => x.Name == input.Name);
                if (existingOrg != null)
                {
                    return ApiResult<SpecialSubjectDto>.Fail(ResultCode.Fail, "专题名已存在");
                }
                //创建专题
                var organization = ObjectMapper.Map<UpdateSpecialSubjectDto, SpecialSubjectModel>(input);
                //插入数据库
                var organizationDto = await _specialSubjectRepository.InsertAsync(organization);
                //映射
                var result = ObjectMapper.Map<SpecialSubjectModel, SpecialSubjectDto>(organizationDto);
                //返回
                return ApiResult<SpecialSubjectDto>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError($"创建专题失败: {ex.Message}");
                return ApiResult<SpecialSubjectDto>.Fail(ResultCode.Fail, $"创建专题失败: {ex.Message}");
            } 
        }
        /// <summary>
        /// 专题级别+add
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        public async  Task<ApiResult<CategoryModelDto>> CreateCategoryAsync(UpdateCategoryDto Dto)
        {
            try
            {
                // 验证专题级别名是否已存在
                var existingOrg = await _categoryRepository.FirstOrDefaultAsync(x => x.CategoryName == Dto.CategoryName);
                if (existingOrg != null)
                {
                    return ApiResult<CategoryModelDto>.Fail(ResultCode.Fail, "专题级别已存在");
                }
                //创建专题级别System.NullReferenceException:“Object reference not set to an instance of an object.
                var organization = ObjectMapper.Map<UpdateCategoryDto, CategoryModel>(Dto);
                //插入数据库
                var organizationDto = await _categoryRepository.InsertAsync(organization);
                //映射
                var result = ObjectMapper.Map<CategoryModel, CategoryModelDto> (organizationDto);
                //返回
                return ApiResult<CategoryModelDto>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError($"创建专题级别失败: {ex.Message}");
                return ApiResult<CategoryModelDto>.Fail(ResultCode.Fail, $"创建专题级别失败: {ex.Message}");
            }

        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        public async Task<ApiResult> DeleteGuidAsync(List<Guid> guids)
        {
            try
            {
                Guid[] ids = guids.ToArray(); 
                foreach (var id in ids)
                {
                    var course = await _specialSubjectRepository.FirstOrDefaultAsync(x => x.Id == id);
                    //删除
                    await _specialSubjectRepository.DeleteAsync(course);
                }
                return ApiResult.Success(ResultCode.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("批量删除SpecialSubjec失败" + ex);
                return ApiResult<SubjectDto>.Fail(ResultCode.Fail, $"批量删除SpecialSubjec失败: {ex.Message}");
            }
        }
        /// <summary>
        /// 反填专题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<SpecialSubjectDto>> GetAsync(Guid id)
        {
            try
            {
                var course = await _specialSubjectRepository.FirstOrDefaultAsync(x => x.Id == id);
                var result = ObjectMapper.Map<SpecialSubjectModel, SpecialSubjectDto>(course); return ApiResult<SpecialSubjectDto>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("反填---专题失败" + ex);
                return ApiResult<SpecialSubjectDto>.Fail(ResultCode.Fail, $"反填---专题异常: {ex.Message}");
            }
        }
        /// <summary>
        /// 下拉专题级别
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<List<CategoryModelDto>>> GetCategoryAsync()
        {
            try
            {
                // 正确获取可查询接口
                var queryable = await _categoryRepository.GetListAsync();
                // 正确映射集合类型
                var results = ObjectMapper.Map<List<CategoryModel>, List<CategoryModelDto>>(queryable);
                //  返回成功结果
                return ApiResult<List<CategoryModelDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                // 添加日志记录
                Logger.LogError(ex, "专题级别获取失败");
                throw;
            }
        }
        /// <summary>
        /// 查询专题--分页+查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<ApiPaging<List<SpecialSubjectandCategory>>>> GetListAsync([FromQuery] SpecialSubjectSearchDto search)
        {
            try
            {
                // 构建专题查询
                var existingSub = await _specialSubjectRepository.GetQueryableAsync();
                //专题级别查询
                var levels = await _categoryRepository.GetQueryableAsync();
                // 专题名称查询
                existingSub = existingSub.WhereIf(!string.IsNullOrEmpty(search.Name), x => x.Name.Contains(search.Name));
                // 专题名称查询
                if (search.CategoryId != null)
                {
                    existingSub = existingSub.Where(x => x.CategoryId == search.CategoryId);
                } 
                // 专题名称查询
                existingSub = existingSub.WhereIf(!string.IsNullOrEmpty(search.Teacher), x => x.Teacher.Contains(search.Teacher));

                //两表联查
                var linq = from org in existingSub
                           join level in levels
                           on org.CategoryId equals level.Id
                           select new SpecialSubjectandCategory
                           {
                               Id=org.Id,
                               CategoryName=level.CategoryName,
                               Name=org.Name,
                               CategoryId=org.CategoryId,
                               LogoPath=org.LogoPath,
                               Teacher=org.Teacher,
                               Brief=org.Brief,
                               Details=org.Details,
                               AchievementDisplay=org.AchievementDisplay
                           };
                // 使用ABP自带分页方法 
                var page = existingSub.PageResult(search.PageIndex, search.PageSize);
                // 映射
                //var organizationDto = ObjectMapper.Map<List<>,List<LevelDto>>(page);
                var result = new ApiPaging<List<SpecialSubjectandCategory>>
                {
                    TotleCount = page.RowCount,
                    TotlePage = (int)Math.Ceiling(page.RowCount * 1.0 / search.PageSize),
                    Data = linq.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToList()
                };
                return ApiResult<ApiPaging<List<SpecialSubjectandCategory>>>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError("查询分页---专题异常" + ex);
                return ApiResult<ApiPaging<List<SpecialSubjectandCategory>>>.Fail(ResultCode.Fail, $"查询分页---专题异常: {ex.Message}");
            }

        }
        /// <summary>
        /// 检查专题名称更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResult<SpecialSubjectDto>> UpdateAsync(Guid id, SpecialSubjectDto input)
        {
            try
            {
                // 检查专题名称是否存在
                var organization = await _specialSubjectRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (organization == null)
                {
                    return ApiResult<SpecialSubjectDto>.Fail(ResultCode.Fail, "专题不存在");
                }

                // 检查机构名是否重复（排除自己）
                var existingOrg = await _specialSubjectRepository.FirstOrDefaultAsync(x => x.Name == input.Name && x.Id != id);
                if (existingOrg != null)
                {
                    return ApiResult<SpecialSubjectDto>.Fail(ResultCode.Fail, "专题名称已存在");
                } 

                ObjectMapper.Map(input, organization);

                //organization.Id = id;
                // 返回更新后信息 
                //result.Id = id;
                var a = await _specialSubjectRepository.UpdateAsync(organization);
                if (a == null)
                {
                    return ApiResult<SpecialSubjectDto>.Fail(ResultCode.Fail, "更新专题失败");
                }
                var result = ObjectMapper.Map<SpecialSubjectModel, SpecialSubjectDto>(organization);
                return ApiResult<SpecialSubjectDto>.Success(ResultCode.Ok, result);
            }
            catch (Exception ex)
            {
                return ApiResult<SpecialSubjectDto>.Fail(ResultCode.Fail, $"更新专题失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 下拉专题
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<XiAsepecialSubjectDto>>> XiASpecialSubjectAsync()
        {
            try
            {
                // 正确获取可查询接口
                var queryable = await _specialSubjectRepository.GetListAsync();
                // 正确映射集合类型
                var results = ObjectMapper.Map<List<SpecialSubjectModel>, List<XiAsepecialSubjectDto>>(queryable);
                //  返回成功结果
                return ApiResult<List<XiAsepecialSubjectDto>>.Success(ResultCode.Ok, results);
            }
            catch (Exception ex)
            {
                // 添加日志记录
              //  Logger.LogError(ex, "专题级别获取失败");
                throw;
            }
        }
    }
    
}
