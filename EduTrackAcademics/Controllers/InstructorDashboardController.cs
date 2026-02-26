using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exception;
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

		// MODULE

		[HttpPost("module")]
		public async Task<IActionResult> CreateModule(ModuleDTO dto)
		{
			var result = await _service.CreateModuleAsync(dto);

			return Ok(new
			{
				message = result.message,
				moduleId = result.module.ModuleID,
				courseId = result.module.CourseID,
				name = result.module.Name,
				sequenceOrder = result.module.SequenceOrder,
				learningObjectives = result.module.LearningObjectives
			});
		}

		[HttpGet("modules/{courseId}")]
		public async Task<IActionResult> GetModules(string courseId)
		{
			var modules = await _service.GetModulesAsync(courseId);

			if (!modules.Any())
				return NotFound("No modules found for this course");

			return Ok(modules);
		}

		[HttpPut("module/{moduleId}")]
		public async Task<IActionResult> UpdateModule(string moduleId, ModuleDTO dto)
		{
			var message = await _service.UpdateModuleAsync(moduleId, dto);

			if (message == "Module not found")
				return NotFound(message);

			return Ok(new { message });
		}

		// CONTENT

		[HttpPost("content")]
		public async Task<IActionResult> Create(ContentDTO dto)
		{
			try { return Ok(await _service.CreateContentAsync(dto)); }
			catch (ApplicationException ex) { return BadRequest(ex.Message); }
		}

		[HttpGet("content/module/{moduleId}")]
		public async Task<IActionResult> GetByModule(string moduleId)
		{
			return Ok(await _service.GetContentByModuleAsync(moduleId));
		}

		[HttpGet("content/{id}")]
		public async Task<IActionResult> Get(string id)
		{
			try { return Ok(await _service.GetContentAsync(id)); }
			catch (ApplicationException ex) { return NotFound(ex.Message); }
		}

		[HttpPut("content/{id}")]
		public async Task<IActionResult> Update(string id, ContentDTO dto)
		{
			try { return Ok(await _service.UpdateContentAsync(id, dto)); }
			catch (ApplicationException ex) { return BadRequest(ex.Message); }
		}

		[HttpPut("content/publish/{id}")]
		public async Task<IActionResult> Publish(string id)
		{
			try { return Ok(await _service.PublishContentAsync(id)); }
			catch (ApplicationException ex) { return BadRequest(ex.Message); }
		}

		[HttpDelete("content/{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			try { return Ok(await _service.DeleteContentAsync(id)); }
			catch (ApplicationException ex) { return NotFound(ex.Message); }
		}

		// ASSESSMENT

		[HttpPost("assessment")]
		public async Task<IActionResult> CreateAssessment(AssessmentDTO dto)
		=> Ok(await _service.CreateAssessmentAsync(dto));

		[HttpGet("assessment/{id}")]
		public async Task<IActionResult> GetAssessment(string id)
			=> Ok(await _service.GetAssessmentByIdAsync(id));

		[HttpGet("assessments/course/{courseId}")]
		public async Task<IActionResult> GetAssessmentsByCourse(string courseId)
			=> Ok(await _service.GetAssessmentsByCourseAsync(courseId));

		[HttpPut("assessment/{id}")]
		public async Task<IActionResult> UpdateAssessment(string id, AssessmentDTO dto)
			=> Ok(await _service.UpdateAssessmentAsync(id, dto));

		[HttpDelete("assessment/{id}")]
		public async Task<IActionResult> DeleteAssessment(string id)
			=> Ok(await _service.DeleteAssessmentAsync(id));

		// QUESTIONS

		[HttpPost("question")]
		public async Task<IActionResult> AddQuestion(QuestionDTO dto)
	   => Ok(await _service.AddQuestionAsync(dto));

		[HttpGet("question/{id}")]
		public async Task<IActionResult> GetQuestion(string id)
			=> Ok(await _service.GetQuestionByIdAsync(id));

		[HttpGet("questions/assessment/{assessmentId}")]
		public async Task<IActionResult> GetQuestionsByAssessment(string assessmentId)
			=> Ok(await _service.GetQuestionsByAssessmentAsync(assessmentId));

		[HttpPut("question/{id}")]
		public async Task<IActionResult> UpdateQuestion(string id, QuestionDTO dto)
			=> Ok(await _service.UpdateQuestionAsync(id, dto));

		[HttpDelete("question/{id}")]
		public async Task<IActionResult> DeleteQuestion(string id)
			=> Ok(await _service.DeleteQuestionAsync(id));

		// ATTENDANCE

		[HttpPost("attendance")]
		public async Task<IActionResult> MarkAttendance(AttendanceDTO dto)
	=> Ok(await _service.MarkAttendanceAsync(dto));

		[HttpPut("attendance/{id}")]
		public async Task<IActionResult> UpdateAttendance(string id, bool status)
			=> Ok(await _service.UpdateAttendanceAsync(id, status));

		[HttpDelete("attendance/{id}")]
		public async Task<IActionResult> DeleteAttendance(string id)
			=> Ok(await _service.DeleteAttendanceAsync(id));

		[HttpGet("attendance/batch/{batchId}")]
		public async Task<IActionResult> GetByBatch(string batchId)
			=> Ok(await _service.ViewByBatchAsync(batchId));

		[HttpGet("attendance/date")]
		public async Task<IActionResult> GetByDate(DateTime date)
			=> Ok(await _service.ViewByDateAsync(date));

		[HttpGet("attendance/enrollment/{enrollmentId}")]
		public async Task<IActionResult> GetByEnrollment(string enrollmentId)
			=> Ok(await _service.ViewByEnrollmentAsync(enrollmentId));


	}

}

