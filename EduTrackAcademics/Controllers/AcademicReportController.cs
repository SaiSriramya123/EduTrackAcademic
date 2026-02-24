using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicReportController : ControllerBase
    {
        private readonly IAcademicReportService _service;
        public AcademicReportController(IAcademicReportService service)
        {
            _service = service;
        }
        [HttpGet("batches")]
        public IActionResult GetBatches()
        {
            var result = _service.GetBatches();
            return Ok(result);
        }
        [HttpGet("batch-details/{reportId}")]
        public IActionResult GetBatchDetails(string reportId)
        {
            var result = _service.GetBatchDetails(reportId);
            return Ok(result);
        }
    }
}