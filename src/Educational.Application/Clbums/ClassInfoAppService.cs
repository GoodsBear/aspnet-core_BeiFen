using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Educational.Clbums
{
    [ApiExplorerSettings(GroupName = "班级")]
    public class ClassInfoAppService:ApplicationService,IClassInfoAppService
    {

    }
}
