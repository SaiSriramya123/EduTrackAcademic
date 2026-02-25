using EduTrackAcademics.Data;
using EduTrackAcademics.Model;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Controllers
{
	[ApiController]
	[Route("api/instructor")]
	public class InstructorDashboardController : ControllerBase
	{
		private readonly IInstructorService _service;

		public InstructorDashboardController(IInstructorService service)
		{
			_service = service;
		}

		[HttpGet("{instructorId}/batches")]
		public async Task<IActionResult> GetBatches(string instructorId)
		=> Ok(await _service.GetBatches(instructorId));

		[HttpGet("batch/{batchId}/students")]
		public async Task<IActionResult> GetStudents(string batchId)
			=> Ok(await _service.GetStudents(batchId));

		[HttpGet("{instructorId}/dashboard")]
		public async Task<IActionResult> Dashboard(string instructorId)
			=> Ok(await _service.GetDashboard(instructorId));

		[HttpPost("module/add")] 
		public async Task<IActionResult> AddModule([FromBody] Module m) { 
			await _service.AddModuleAsync(m); 
			return Ok("Module added"); 
		}

		[HttpPut("module/update/{id}")] 
		public async Task<IActionResult> UpdateModule(string id, [FromBody] Module m) { 
			await _service.UpdateModuleAsync(id, m); 
			return Ok("Module updated"); 
		}

		[HttpDelete("module/delete/{id}")] 
		public async Task<IActionResult> DeleteModule(string id) { 
			await _service.DeleteModuleAsync(id); 
			return Ok("Deleted"); 
		}

		[HttpGet("course/{courseId}/modules")] 
		public async Task<IActionResult> GetModules(string courseId) 
			=> Ok(await _service.GetModules(courseId));

		[HttpPut("module/{id}/complete")] 
		public async Task<IActionResult> Complete(string id) 
			=> Ok(await _service.CompleteModule(id));

		[HttpPost("content/add")] 
		public async Task<IActionResult> AddContent(Content c) { 
			await _service.AddContent(c); 
			return Ok(); 
		}

		[HttpPut("content/update")] 
		public async Task<IActionResult> UpdateContent(Content c) { 
			await _service.UpdateContent(c); 
			return Ok(); 
		}

		[HttpDelete("content/{id}")] 
		public async Task<IActionResult> DeleteContent(string id) { 
			await _service.DeleteContent(id); 
			return Ok(); 
		}

		[HttpGet("module/{id}/content")] 
		public async Task<IActionResult> GetContent(string id) 
			=> Ok(await _service.GetContent(id));

		[HttpPost("assessment/add")] 
		public async Task<IActionResult> AddAssessment(Assessment a) { 
			await _service.AddAssessment(a); 
			return Ok(); 
		}

		[HttpPut("assessment/update")] 
		public async Task<IActionResult> UpdateAssessment(Assessment a) { 
			await _service.UpdateAssessment(a); 
			return Ok(); 
		}

		[HttpDelete("assessment/{id}")] 
		public async Task<IActionResult> DeleteAssessment(string id) { 
			await _service.DeleteAssessment(id); 
			return Ok(); 
		}

		[HttpGet("course/{courseId}/assessments")] 
		public async Task<IActionResult> GetAssessments(string courseId) 
			=> Ok(await _service.GetAssessments(courseId));

		[HttpGet("assessment/{id}/questions")] 
		public async Task<IActionResult> GetQuestions(string id) 
			=> Ok(await _service.GetQuestions(id));

		[HttpPut("assessment/evaluate")] 
		public async Task<IActionResult> Evaluate(string id, int marks, string feedback) { 
			await _service.EvaluateAssessment(id, marks, feedback); 
			return Ok(); 
		}

		[HttpPost("attendance/mark")] 
		public async Task<IActionResult> Mark(Attendance a) { 
			await _service.MarkAttendance(a); 
			return Ok(); 
		}

		[HttpPut("attendance/{id}")] 
		public async Task<IActionResult> UpdateAtt(string id, Attendance a) { 
			await _service.UpdateAttendance(id, a); 
			return Ok(); 
		}

		[HttpDelete("attendance/{id}")] 
		public async Task<IActionResult> DeleteAtt(string id, string reason) { 
			await _service.DeleteAttendance(id, reason); 
			return Ok(); 
		}

		[HttpGet("attendance/{batchId}")] 
		public async Task<IActionResult> GetAtt(string batchId) 
			=> Ok(await _service.GetAttendance(batchId));

		[HttpGet("attendance/report/{batchId}")] 
		public async Task<IActionResult> Report(string batchId) 
			=> Ok(await _service.GetAttendanceReport(batchId));

		[HttpGet("attendance/irregular/{batchId}")] 
		public async Task<IActionResult> Irregular(string batchId) 
			=> Ok(await _service.GetIrregularStudents(batchId));
	}

}

