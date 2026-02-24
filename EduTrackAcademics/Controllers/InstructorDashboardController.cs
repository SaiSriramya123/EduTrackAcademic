using EduTrackAcademics.Data;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InstructorDashboardController : ControllerBase
	{
		private readonly EduTrackAcademicsContext _context;

		public InstructorDashboardController(EduTrackAcademicsContext context)
		{
			_context = context;
		}

		// ===============================
		// VIEW ASSIGNED BATCHES
		// ===============================
		[HttpGet("{instructorId}/batches")]
		public IActionResult GetBatches(string instructorId)
		{
			var batches = DummyInstructorData.GetBatches()
				.Where(b => b.InstructorId == instructorId)
				.ToList();

			return Ok(batches);
		}

		// ===============================
		// VIEW STUDENTS IN A BATCH
		// ===============================
		[HttpGet("batch/{batchId}/students")]
		public IActionResult GetStudents(string batchId)
		{
			var students = DummyInstructorData.GetStudents()
				.Where(s => s.BatchId == batchId)
				.Select(s => new
				{
					s.Student.StudentId,
					s.Student.StudentName,
					s.Student.StudentEmail
				}).ToList();

			return Ok(students);
		}

		// ===============================
		// VIEW MODULES FOR A COURSE
		// ===============================
		[HttpGet("course/{courseId}/modules")]
		public IActionResult GetModules(string courseId)
		{
			var modules = DummyInstructorData.GetModules()
				.Where(m => m.CourseID == courseId)
				.ToList();

			return Ok(modules);
		}

		// ===============================
		// VIEW CONTENT INSIDE MODULE
		// ===============================
		[HttpGet("module/{moduleId}/content")]
		public IActionResult GetContent(string moduleId)
		{
			var content = DummyInstructorData.GetContents()
				.Where(c => c.ModuleID == moduleId)
				.ToList();

			return Ok(content);
		}

		// ===============================
		// VIEW ASSESSMENTS FOR COURSE
		// ===============================
		[HttpGet("course/{courseId}/assessments")]
		public IActionResult GetAssessments(string courseId)
		{
			var assessments = DummyInstructorData.GetAssessments()
				.Where(a => a.CourseID == courseId)
				.ToList();

			return Ok(assessments);
		}

		// ===============================
		// VIEW QUESTIONS OF ASSESSMENT
		// ===============================
		[HttpGet("assessment/{assessmentId}/questions")]
		public IActionResult GetQuestions(string assessmentId)
		{
			var assessment = DummyInstructorData.GetAssessments()
				.FirstOrDefault(a => a.AssessmentID == assessmentId);
			if (assessment == null)
				return NotFound("Assessment Not Found");

			return Ok(assessment.Questions);
		}

		// ===============================
		// EVALUATE STUDENT ASSESSMENT
		// ===============================
		[HttpPut("assessment/evaluate")]
		public IActionResult Evaluate(string assessmentId, int marks, string feedback)
		{
			var assessment = DummyInstructorData.GetAssessments()
				.FirstOrDefault(a => a.AssessmentID == assessmentId);

			if (assessment == null)
				return NotFound();

			assessment.MarksObtained = marks;
			assessment.Feedback = feedback;

			return Ok("Evaluation saved");
		}

		// ===============================
		// MARK ATTENDANCE
		// ===============================
		[HttpPost("attendance/mark")]
		public IActionResult MarkAttendance([FromBody] Attendance attendance)
		{
			DummyInstructorData.Attendances.Add(attendance);
			return Ok("Attendance marked");
		}

		// ===============================
		// VIEW ATTENDANCE BY BATCH
		// ===============================
		[HttpGet("attendance/{batchId}")]
		public IActionResult GetAttendance(string batchId)
		{
			var records = DummyInstructorData.Attendances
				.Where(a => a.BatchID == batchId)
				.ToList();

			return Ok(records);
		}
	}
}
