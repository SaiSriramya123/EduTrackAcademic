using EduTrackAcademics.Data;
using EduTrackAcademics.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Controllers
{
	[ApiController]
	[Route("api/instructor")]
	public class InstructorDashboardController : ControllerBase
	{
		private readonly EduTrackAcademicsContext _context;

		public InstructorDashboardController(EduTrackAcademicsContext context)
		{
			_context = context;
		}

		// =========================================================
		// 1️ VIEW ASSIGNED BATCHES
		// =========================================================
		[HttpGet("{instructorId}/batches")]
		public async Task<IActionResult> GetBatches(string instructorId)
		{
			var batches = await _context.CourseBatches
				.Where(b => b.InstructorId == instructorId)
				.Select(b => new
				{
					b.BatchId,
					b.MaxStudents,
					b.CurrentStudents,
					b.IsActive,
					b.Course.CourseId,
					b.Course.CourseName
				}).ToListAsync();

			return Ok(batches);
		}

		// =========================================================
		// 2️ VIEW STUDENTS IN BATCH
		// =========================================================
		[HttpGet("batch/{batchId}/students")]
		public async Task<IActionResult> GetStudents(string batchId)
		{
			var students = await _context.StudentBatchAssignments
				.Where(s => s.BatchId == batchId)
				.Select(s => new
				{
					s.Student.StudentId,
					s.Student.StudentName,
					s.Student.StudentEmail,
					s.Student.StudentPhone
				}).ToListAsync();

			return Ok(students);
		}

		// =========================================================
		// 3️ FULL DASHBOARD
		// =========================================================
		[HttpGet("{instructorId}/dashboard")]
		public async Task<IActionResult> GetDashboard(string instructorId)
		{
			var dashboard = await (
				from batch in _context.CourseBatches
				where batch.InstructorId == instructorId

				join course in _context.Course
					on batch.CourseId equals course.CourseId

				join sba in _context.StudentBatchAssignments
					on batch.BatchId equals sba.BatchId into sbagroup

				select new
				{
					batch.BatchId,
					batch.CourseId,
					CourseName = course.CourseName,

					Students = sbagroup.Select(s => new
					{
						s.Student.StudentId,
						s.Student.StudentName,
						s.Student.StudentEmail,
						s.Student.StudentPhone
					}).ToList()
				}
			).ToListAsync();

			return Ok(dashboard);
		}


		// =========================================================
		// 4️ ADD MODULE
		// =========================================================
		[HttpPost("module/add")]
		public async Task<IActionResult> AddModule(Module module)
		{
			_context.Modules.Add(module);
			await _context.SaveChangesAsync();
			return Ok("Module created");
		}

		// =========================================================
		// 5️ UPDATE MODULE
		// =========================================================
		[HttpPut("module/update")]
		public async Task<IActionResult> UpdateModule(Module module)
		{
			_context.Modules.Update(module);
			await _context.SaveChangesAsync();
			return Ok("Module updated");
		}

		// =========================================================
		// 6️ DELETE MODULE
		// =========================================================
		[HttpDelete("module/{id}")]
		public async Task<IActionResult> DeleteModule(string id)
		{
			var module = await _context.Modules.FindAsync(id);
			if (module == null) return NotFound();

			_context.Modules.Remove(module);
			await _context.SaveChangesAsync();
			return Ok("Module deleted");
		}

		// =========================================================
		// 7️ GET MODULES FOR COURSE
		// =========================================================
		[HttpGet("course/{courseId}/modules")]
		public async Task<IActionResult> GetModules(string courseId)
		{
			var modules = await _context.Modules
				.Where(m => m.CourseID == courseId)
				.Select(m => new
				{
					m.ModuleID,
					m.Name,
					m.SequenceOrder
				}).ToListAsync();

			return Ok(modules);
		}

		// =========================================================
		// 8️ COMPLETE MODULE
		// =========================================================
		[HttpPut("module/{moduleId}/complete")]
		public async Task<IActionResult> CompleteModule(string moduleId)
		{
			var contentCount = await _context.Contents
				.CountAsync(c => c.ModuleID == moduleId);

			if (contentCount == 0)
				return BadRequest("Module has no content");

			return Ok("Module completed successfully");
		}

		// =========================================================
		// 9️ ADD CONTENT
		// =========================================================
		[HttpPost("content/add")]
		public async Task<IActionResult> AddContent(Content content)
		{
			_context.Contents.Add(content);
			await _context.SaveChangesAsync();
			return Ok("Content added");
		}

		// =========================================================
		// 10 UPDATE CONTENT
		// =========================================================
		[HttpPut("content/update")]
		public async Task<IActionResult> UpdateContent(Content content)
		{
			_context.Contents.Update(content);
			await _context.SaveChangesAsync();
			return Ok("Content updated");
		}

		// =========================================================
		// 1️1 DELETE CONTENT
		// =========================================================
		[HttpDelete("content/{id}")]
		public async Task<IActionResult> DeleteContent(string id)
		{
			var content = await _context.Contents.FindAsync(id);
			if (content == null) return NotFound();

			_context.Contents.Remove(content);
			await _context.SaveChangesAsync();
			return Ok("Content deleted");
		}

		// =========================================================
		// 1️2 VIEW CONTENT IN MODULE
		// =========================================================
		[HttpGet("module/{moduleId}/content")]
		public async Task<IActionResult> GetContent(string moduleId)
		{
			var content = await _context.Contents
				.Where(c => c.ModuleID == moduleId)
				.Select(c => new
				{
					c.ContentID,
					c.Title,
					c.ContentType,
					c.ContentURI
				}).ToListAsync();

			return Ok(content);
		}

		// =========================================================
		// 1️3 ADD ASSESSMENT
		// =========================================================
		[HttpPost("assessment/add")]
		public async Task<IActionResult> AddAssessment(Assessment a)
		{
			_context.Assessments.Add(a);
			await _context.SaveChangesAsync();
			return Ok("Assessment created");
		}

		// =========================================================
		// 14 DELETE ASSESSMENT
		// =========================================================
		[HttpDelete("assessment/{id}")]
		public async Task<IActionResult> DeleteAssessment(string id)
		{
			var a = await _context.Assessments.FindAsync(id);
			if (a == null) return NotFound();

			_context.Assessments.Remove(a);
			await _context.SaveChangesAsync();
			return Ok("Assessment deleted");
		}

		// =========================================================
		// 15 GET ASSESSMENTS FOR COURSE
		// =========================================================
		[HttpGet("course/{courseId}/assessments")]
		public async Task<IActionResult> GetAssessments(string courseId)
		{
			var assessments = await _context.Assessments
				.Where(a => a.CourseID == courseId)
				.Select(a => new
				{
					a.AssessmentID,
					a.Type,
					a.MaxMarks,
					a.DueDate,
					a.Status
				}).ToListAsync();

			return Ok(assessments);
		}

		// =========================================================
		// 16 GET QUESTIONS
		// =========================================================
		[HttpGet("assessment/{assessmentId}/questions")]
		public async Task<IActionResult> GetQuestions(string assessmentId)
		{
			var questions = await _context.Questions
				.Where(q => q.AssessmentId == assessmentId)
				.ToListAsync();

			return Ok(questions);
		}

		// =========================================================
		// 17 EVALUATE ASSESSMENT
		// =========================================================
		[HttpPut("assessment/evaluate")]
		public async Task<IActionResult> EvaluateAssessment(string assessmentId, int marks, string feedback)
		{
			var assessment = await _context.Assessments.FindAsync(assessmentId);
			if (assessment == null) return NotFound();

			if (marks > assessment.MaxMarks)
				return BadRequest("Marks exceed maximum");

			assessment.MarksObtained = marks;
			assessment.Feedback = feedback;
			assessment.Status = "Evaluated";

			await _context.SaveChangesAsync();
			return Ok("Assessment evaluated");
		}

		// =========================================================
		// 18 MARK ATTENDANCE
		// =========================================================
		[HttpPost("attendance/mark")]
		public async Task<IActionResult> MarkAttendance(Attendance attendance)
		{
			attendance.SessionDate = DateTime.Now;

			_context.Attendances.Add(attendance);
			await _context.SaveChangesAsync();

			return Ok("Attendance marked");
		}

		// =========================================================
		// 19 UPDATE ATTENDANCE
		// =========================================================
		[HttpPut("attendance/{attendanceId}")]
		public async Task<IActionResult> UpdateAttendance(string attendanceId, Attendance updated)
		{
			var record = await _context.Attendances.FindAsync(attendanceId);
			if (record == null) return NotFound();

			record.Status = updated.Status;
			record.Mode = updated.Mode;
			record.UpdateReason = updated.UpdateReason;
			record.UpdatedOn = DateTime.Now;

			await _context.SaveChangesAsync();
			return Ok("Attendance updated");
		}

		// =========================================================
		// 20 DELETE ATTENDANCE (SOFT DELETE)
		// =========================================================
		[HttpDelete("attendance/{attendanceId}")]
		public async Task<IActionResult> DeleteAttendance(string attendanceId, string reason)
		{
			var record = await _context.Attendances.FindAsync(attendanceId);
			if (record == null) return NotFound();

			record.IsDeleted = true;
			record.DeletionReason = reason;
			record.DeletionDate = DateTime.Now;

			await _context.SaveChangesAsync();
			return Ok("Attendance deleted");
		}

		// =========================================================
		// 21 VIEW ATTENDANCE
		// =========================================================
		[HttpGet("attendance/{batchId}")]
		public async Task<IActionResult> GetAttendance(string batchId)
		{
			var records = await _context.Attendances
				.Where(a => a.BatchId == batchId && !a.IsDeleted)
				.ToListAsync();

			return Ok(records);
		}

		// =========================================================
		// 22 ATTENDANCE REPORT
		// =========================================================
		[HttpGet("attendance/report/{batchId}")]
		public async Task<IActionResult> AttendanceReport(string batchId)
		{
			var report = await _context.Attendances
				.Where(a => a.BatchId == batchId && !a.IsDeleted)
				.Include(a => a.StudentBatchAssignment)
				.ThenInclude(sba => sba.Student)
				.GroupBy(a => new
				{
					a.StudentBatchAssignment.Student.StudentId,
					a.StudentBatchAssignment.Student.StudentName
				})
				.Select(g => new
				{
					StudentId = g.Key.StudentId,
					StudentName = g.Key.StudentName,
					Present = g.Count(x => x.Status),
					Absent = g.Count(x => !x.Status)
				})
				.ToListAsync();

			return Ok(report);
		}

		// =========================================================
		// 23 IRREGULAR STUDENTS
		// =========================================================
		[HttpGet("attendance/irregular/{batchId}")]
		public async Task<IActionResult> IrregularStudents(string batchId)
		{
			var irregular = await _context.Attendances
				.Where(a => a.BatchId == batchId && !a.IsDeleted)
				.Include(a => a.StudentBatchAssignment)
				.ThenInclude(sba => sba.Student)
				.GroupBy(a => new
				{
					a.StudentBatchAssignment.Student.StudentId,
					a.StudentBatchAssignment.Student.StudentName
				})
				.Select(g => new
				{
					StudentId = g.Key.StudentId,
					StudentName = g.Key.StudentName,
					Absences = g.Count(x => !x.Status)
				})
				.Where(x => x.Absences > 3)
				.ToListAsync();

			return Ok(irregular);
		}

	}
}
