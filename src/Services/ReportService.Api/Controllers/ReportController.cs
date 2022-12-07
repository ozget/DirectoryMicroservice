using Microsoft.AspNetCore.Mvc;
using ReportService.Application.Service;
using System.Threading.Tasks;

namespace ReportService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_reportService.GetAllReport());
        }
    }
}
