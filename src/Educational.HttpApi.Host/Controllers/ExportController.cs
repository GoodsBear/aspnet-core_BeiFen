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

        public ExportController(IStaffServices staffServices)
        {
            this.staffServices = staffServices;
        }
        [HttpGet]
        public async Task<IActionResult> ExportStaffDirect()
        {
            var fileDto = await staffServices.GetExportStaffList();

            return File(fileDto.data.FileContent,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileDto.data.FileName);
        }
    }
}
