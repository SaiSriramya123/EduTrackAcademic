using EduTrackAcademics.DTO;
using EduTrackAcademics.Service;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
	[ApiController]
	[Route("api/coordinator")]
	public class CoordinatorDashboardController : ControllerBase
	{
		private readonly ICoordinatorDashboardService _service;

		public CoordinatorDashboardController(ICoordinatorDashboardService service)
		{
			_service = service;
		}

		[HttpGet("programs")]
		public IActionResult GetPrograms()
		{
			return Ok(_service.GetPrograms());
		}

		[HttpGet("program/{programId}/years")]
		public IActionResult GetAcademicYears(string programId)
		{
			return Ok(_service.GetAcademicYears(programId));
		}

		[HttpPost("course")]
		public IActionResult AddCourse([FromBody] CourseDTO dto)
		{
			return Ok(_service.AddCourse(dto));
		}

		[HttpGet("academic-year/{yearId}/courses")]
		public IActionResult GetCourses(string yearId)
		{
			return Ok(_service.GetCourses(yearId));
		}

		[HttpGet("students")]
		public IActionResult GetStudents(string qualification, string program, int year)
		{
			return Ok(_service.GetStudents(qualification, program, year));
		}

		[HttpGet("instructors")]
		public IActionResult GetInstructors(string skill)
		{
			return Ok(_service.GetInstructors(skill));
		}

		[HttpGet("batches")]
		public IActionResult GetBatches(string program, int year)
		{
			return Ok(_service.GetBatches(program, year));
		}

		[HttpGet("batch-count")]
		public IActionResult GetBatchCount(string program, int year)
		{
			return Ok(_service.GetBatchCount(program, year));
		}

		[HttpGet("batch/{batchId}/students")]
		public IActionResult GetStudentsInBatch(string batchId)
		{
			return Ok(_service.GetStudentsInBatch(batchId));
		}

		[HttpPost("assign-batches")]
		public IActionResult AssignBatches([FromBody] AutoAssignBatchDTO dto)
		{
			return Ok(_service.AssignBatches(dto));
		}

		[HttpPost("assign-single-batch")]
		public IActionResult AssignSingleBatch([FromBody] AutoAssignBatchDTO dto)
		{
			return Ok(_service.AssignSingleBatch(dto));
		}

		[HttpGet("instructor/{instructorId}/batches")]
		public IActionResult GetInstructorBatches(string instructorId)
		{
			return Ok(_service.GetInstructorBatches(instructorId));
		}

		[HttpGet("instructor/{instructorId}/dashboard")]
		public IActionResult InstructorDashboard(string instructorId)
		{
			return Ok(_service.InstructorDashboard(instructorId));
		}
	}
}
