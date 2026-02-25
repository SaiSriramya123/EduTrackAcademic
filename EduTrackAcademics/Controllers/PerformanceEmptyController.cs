using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceEmptyController : ControllerBase
    {
        //this part is done to connect this controller to the database to fetch the data.
        private readonly IPerformanceService _service;
        public PerformanceEmptyController(IPerformanceService service)
        {
            _service = service;
        }

        [HttpGet("completion/{enrollmentId}")]
        public IActionResult GetCompletionPercentage(int enrollmentId)
        {
            return Ok(_service.GetCompletionPercentage(enrollmentId));
        }

        [HttpGet("average/{enrollmentId}")]
        public IActionResult GetAverageScore(int enrollmentId)
        {
            var result= _service.GetAverageScore(enrollmentId);
            return Ok(result);
        }

        [HttpGet("lastupdated/{enrollmentId}")]
        public IActionResult GetLastUpdated(int enrollmentId)
        {
            var result= _service.GetLastModifiedDate(enrollmentId);
            return Ok(result);
        }
        [HttpGet("instructor-batches/{instructorId}")]

        public IActionResult GetInstructorBatches(int instructorId)

        {

            var result = _service.GetInstructorBatches(instructorId);

            return Ok(result);

        }

        [HttpGet("batch-performance/{batchId}")]

        public IActionResult GetBatchPerformance(int batchId)

        {

            var result = _service.GetBatchPerformance(batchId);

            return Ok(result);

        }

    }
}
   