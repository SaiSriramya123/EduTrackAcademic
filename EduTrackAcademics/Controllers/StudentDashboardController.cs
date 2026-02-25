using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentDashboardController : ControllerBase
	{
		//dependency injection of service
		private readonly IStudentDashboardService _service;
		public StudentDashboardController(IStudentDashboardService service)
		{
			_service = service;
		}

		[HttpGet("student-details/{studentId}")]
		public async Task<IActionResult> GetStudentDetails(string studentId)
		{
			try
			{
				var student = await _service.GetStudentDetails(studentId);

				return Ok(new
				{
					StudentName = student.StudentName,
					StudentEmail = student.StudentEmail,
					StudentQualification = student.StudentQualification,
					StudentProgram = student.StudentProgram,
					StudentAcademicYear = student.StudentAcademicYear,
				});
			}
			catch (ArgumentException ex)
			{
				// Student not found
				return NotFound(new { Message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				// Business logic error (e.g., invalid state)
				return BadRequest(new { Message = ex.Message });
			}
			catch (System.Exception ex)
			{
				// Unexpected error
				return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
			}
		}

		[HttpGet("credit-points/{studentId}")]
		public async Task<IActionResult> GetCreditPoints(string studentId)
		{
			try
			{
				var credits = await _service.GetCreditPointsAsync(studentId);

				return Ok(new
				{
					StudentId = studentId,
					TotalCredits = credits
				});
			}
			catch (ArgumentException ex)
			{
				// Student not found or invalid input
				return NotFound(new { Message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				// Business logic error (e.g., no completed enrollments)
				return BadRequest(new { Message = ex.Message });
			}
			catch (System.Exception ex)
			{
				// Unexpected error
				return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
			}
		}

		[HttpGet("assignment-due")]
		public async Task<IActionResult> GetAssignmentDue(string studentId, string courseId)
		{
			try
			{
				var assignment = await _service.GetAssignmentDetailsForStudentAsync(studentId, courseId);

				return Ok(new
				{
					AssignmentDue = assignment.DueDate,
					AssignmentType = assignment.Type,
					CourseName = assignment.CourseName
				});
			}
			catch (ArgumentException ex)
			{
				return NotFound(new { Message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
			}
		}

		[HttpGet("audit-log/{studentId}")]
		public async Task<IActionResult> GetAuditLog(string studentId)
		{
			try
			{
				var log = await _service.GetAuditLogAsync(studentId);

				return Ok(new
				{
					StudentId = log.StudentId,
					TimeSpent = log.TimeSpent.ToString(),
					LoginDate = log.Date.ToShortDateString()
				});
			}
			catch (ArgumentException ex)
			{
				return NotFound(new { Message = ex.Message });
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
			}
		}


	}
}
