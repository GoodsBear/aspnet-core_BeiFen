using Educational.Positions;
using Educational.Staffs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Educational.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IStaffServices staffServices;
        private readonly IPositionAppService positionAppService;

        public ExportController(IStaffServices staffServices,IPositionAppService positionAppService)
        {
            this.staffServices = staffServices;
            this.positionAppService = positionAppService;
        }

    }
}
