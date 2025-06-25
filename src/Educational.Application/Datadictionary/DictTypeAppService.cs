using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Educational.Datadictionary
{
    public class DictTypeAppService : ApplicationService, IDictTypeAppService
    {
        private readonly IRepository<DictType, long> dictTypeRepository;

        public DictTypeAppService(IRepository<DictType, long> dictTypeRepository)
        {
            this.dictTypeRepository = dictTypeRepository;
        }

        public async Task<ApiResult<List<DictTypeDto>>> GetListAsync()
        {
            var items = await dictTypeRepository.GetListAsync();
            var itemdto=ObjectMapper.Map<List<DictType>, List<DictTypeDto>>(items);
            return ApiResult<List<DictTypeDto>>.Success(ResultCode.Ok, itemdto);
        }

    }
}
