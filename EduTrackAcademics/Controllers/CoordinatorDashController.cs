using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Controllers
{
	[ApiController]
	[Route("api/coordinator")]
	public class CoordinatorDashboardController : ControllerBase
	{
		private readonly EduTrackAcademicsContext _context;

		public CoordinatorDashboardController(EduTrackAcademicsContext context)
		{
			_context = context;
		}

		// ===============================
		// 1️⃣ GET PROGRAMS
		// ===============================
		[HttpGet("programs")]
		public IActionResult GetPrograms()
		{
			var programs = _context.Programs
				.Select(p => new { p.ProgramId, p.ProgramName, p.QualificationId })
				.ToList();

			return Ok(programs);
		}

		// ===============================
		// 2️⃣ GET ACADEMIC YEARS BY PROGRAM
		// ===============================
		[HttpGet("program/{programId}/years")]
		public IActionResult GetAcademicYears(string programId)
		{
			var years = _context.AcademicYear
				.Where(y => y.ProgramId == programId)
				.Select(y => new
				{
					y.AcademicYearId,
					y.YearNumber
				})
				.ToList();

			return Ok(years);
		}

		// ===============================
		// 3️⃣ ADD COURSE
		// ===============================
		[HttpPost("course")]
		public IActionResult AddCourse([FromBody] CourseDTO dto)
		{
			var year = _context.AcademicYear
				.FirstOrDefault(y => y.AcademicYearId == dto.AcademicYearId);

			if (year == null)
				return BadRequest("Academic year not found");

			var course = new Course
			{
				CourseId = $"C{_context.Course.Count() + 1:D3}",
				CourseName = dto.CourseName,
				Credits = dto.Credits,
				DurationInWeeks = dto.DurationInWeeks,
				AcademicYearId = dto.AcademicYearId
			};

			_context.Course.Add(course);
			_context.SaveChanges();

			return Ok(new
			{
				Message = "Course added successfully",
				course.CourseId
			});
		}

		// ===============================
		// 4️⃣ GET COURSES BY ACADEMIC YEAR
		// ===============================
		[HttpGet("academic-year/{yearId}/courses")]
		public IActionResult GetCourses(string yearId)
		{
			var courses = _context.Course
				.Where(c => c.AcademicYearId == yearId)
				.Select(c => new
				{
					c.CourseId,
					c.CourseName,
					c.Credits,
					c.DurationInWeeks
				})
				.ToList();

			return Ok(courses);
		}

		// ===============================
		// 5️⃣ GET STUDENTS BY QUALIFICATION + PROGRAM
		// ===============================
		[HttpGet("students")]
		public IActionResult GetStudents(string qualification, string program)
		{
			var students = _context.Student
				.Where(s =>
					s.StudentQualification == qualification &&
					s.StudentProgram == program)
				.Select(s => new
				{
					s.StudentId,
					s.StudentName,
					s.StudentEmail
				})
				.ToList();

			return Ok(students);
		}

		// ===============================
		// 6️⃣ GET INSTRUCTORS BY SKILL
		// ===============================
		[HttpGet("instructors")]
		public IActionResult GetInstructors(string skill)
		{
			var instructors = _context.Instructor
				.Where(i => i.InstructorSkills.Contains(skill))
				.Select(i => new
				{
					i.InstructorId,
					i.InstructorName,
					i.InstructorSkills
				})
				.ToList();

			return Ok(instructors);
		}

		// ===============================
		// 7️⃣ AUTO ASSIGN STUDENTS → BATCHES
		// ===============================
		[HttpPost("auto-assign-batches")]
		public IActionResult AutoAssignStudentsToBatches([FromBody] AutoAssignBatchDTO dto)
		{
			var students = _context.Student
				.Where(s =>
					s.StudentQualification == dto.Qualification &&
					s.StudentProgram == dto.Program)
				.ToList();

			if (!students.Any())
				return BadRequest("No students found");

			int assignedCount = 0;

			while (students.Any())
			{
				var batch = _context.CourseBatches
					.FirstOrDefault(b =>
						b.CourseId == dto.CourseId &&
						b.IsActive &&
						b.CurrentStudents < b.MaxStudents);

				if (batch == null)
				{
					batch = new CourseBatch
					{
						BatchId = $"C{(_context.Coordinator.Count() + 1):D3}",
						CourseId = dto.CourseId,
						InstructorId = dto.InstructorId,
						MaxStudents = 20,
						CurrentStudents = 0,
						IsActive = true
					};

					_context.CourseBatches.Add(batch);
					_context.SaveChanges();
				}

				int seats = batch.MaxStudents - batch.CurrentStudents;
				var group = students.Take(seats).ToList();

				foreach (var student in group)
				{
					_context.StudentBatchAssignments.Add(new StudentBatchAssignment
					{
						BatchId = batch.BatchId,
						StudentId = student.StudentId
					});

					students.Remove(student);
					batch.CurrentStudents++;
					assignedCount++;
				}

				if (batch.CurrentStudents >= batch.MaxStudents)
					batch.IsActive = false;

				_context.SaveChanges();
			}

			return Ok(new
			{
				Message = "Students assigned batch-wise successfully",
				TotalAssigned = assignedCount
			});
		}

		// ===============================
		// 8️⃣ INSTRUCTOR → VIEW THEIR BATCHES
		// ===============================
		[HttpGet("instructor/{instructorId}/batches")]
		public IActionResult GetInstructorBatches(string instructorId)
		{
			var batches = _context.CourseBatches
				.Where(b => b.InstructorId == instructorId)
				.Select(b => new
				{
					b.BatchId,
					b.CourseId,
					StudentCount = _context.StudentBatchAssignments
						.Count(s => s.BatchId == b.BatchId)
				})
				.ToList();

			return Ok(batches);
		}
	}
}