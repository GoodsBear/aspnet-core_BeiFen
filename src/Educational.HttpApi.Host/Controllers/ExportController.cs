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
        /// <summary>
        /// 成员导出
        /// </summary>
        /// <returns>返回成员EXcel文件</returns>
        [HttpGet]
        public async Task<IActionResult> ExportStaffDirect()
        {
            var fileDto = await staffServices.GetExportStaffList();

            return File(fileDto.data.FileContent,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileDto.data.FileName);
        }
        /// <summary>
        /// 导出职位EXcel
        /// </summary>
        /// <returns>返回职位EXcel文件</returns>
        [HttpGet]
        public async Task<IActionResult> GetExportPositionList()
        {
            var fileDto = await positionAppService.GetExportPositionList();

            return File(fileDto.data.FileContent,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileDto.data.FileName);
        }
    }
}
