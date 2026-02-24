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
		// =====================================================
		[HttpGet("students")]
		public IActionResult GetStudents(string qualification, string program, int year)
		{
			return Ok(_context.Student
				.Where(s =>
					s.StudentQualification == qualification &&
					s.StudentProgram == program &&
					s.Year == year)
				.Select(s => new
				{
					s.StudentId,
					s.StudentName,
					s.StudentEmail
				}).ToList());
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
		[HttpGet("batches")]
		public IActionResult GetBatches(string program, int year)
		{
			return Ok(_context.CourseBatches
				.Include(b => b.Course)
				.Where(b =>
					_context.AcademicYear.Any(y =>
						y.AcademicYearId == b.Course.AcademicYearId &&
						y.YearNumber == year &&
						y.Program.ProgramName == program))
				.Select(b => new
				{
					b.BatchId,
					b.Course.CourseName,
					b.InstructorId,
					b.CurrentStudents,
					b.MaxStudents,
					b.IsActive
				}).ToList());
		}


		// ===============================
		// 7️⃣ AUTO ASSIGN STUDENTS → BATCHES
		// ===============================
		[HttpPost("assign-batches")]
		public IActionResult AssignBatches([FromBody] AutoAssignBatchDTO dto)
		{
			var students = _context.Student
				.Where(s =>
					s.StudentQualification == dto.Qualification &&
					s.StudentProgram == dto.Program &&
					s.Year == dto.Year &&
					!_context.StudentBatchAssignments.Any(a =>
						a.StudentId == s.StudentId &&
						a.Batches.CourseId == dto.CourseId))
				.OrderBy(s => s.StudentId)
				.ToList();

			if (!students.Any())
				return BadRequest("No students found");

			int batchCounter = _context.CourseBatches.Count() + 1;
			int assigned = 0;

			for (int i = 0; i < students.Count; i += dto.BatchSize)
			{
				string batchId = $"B{batchCounter:D3}";

				var batch = new CourseBatch
				{
					BatchId = batchId,
					CourseId = dto.CourseId,
					InstructorId = dto.InstructorId,
					MaxStudents = dto.BatchSize,
					CurrentStudents = 0,
					IsActive = true
				};

				_context.CourseBatches.Add(batch);
				_context.SaveChanges();

				var group = students.Skip(i).Take(dto.BatchSize).ToList();

				foreach (var student in group)
				{
					_context.StudentBatchAssignments.Add(new StudentBatchAssignment
					{
						BatchId = batchId,
						StudentId = student.StudentId
					});

					batch.CurrentStudents++;
					assigned++;
				}

				batch.IsActive = batch.CurrentStudents < batch.MaxStudents;
				_context.SaveChanges();

				batchCounter++;
			}

			return Ok(new
			{
				Message = "Batch assignment completed",
				AssignedStudents = assigned
			});
		}

		// =====================================================
		// 8️⃣ INSTRUCTOR → VIEW BATCHES
		// =====================================================
		[HttpGet("instructor/{instructorId}/batches")]
		public IActionResult GetInstructorBatches(string instructorId)
		{
			return Ok(_context.CourseBatches
				.Where(b => b.InstructorId == instructorId)
				.Select(b => new
				{
					b.BatchId,
					b.Course.CourseName,
					b.MaxStudents,
					b.CurrentStudents,
					b.IsActive
				})
				.ToList());
		}
		[HttpGet("batch-count")]
		public IActionResult GetBatchCount(string program, int year)
		{
			int count = _context.CourseBatches
				.Include(b => b.Course)
				.Count(b =>
					_context.AcademicYear.Any(y =>
						y.AcademicYearId == b.Course.AcademicYearId &&
						y.YearNumber == year &&
						y.Program.ProgramName == program));

			return Ok(new
			{
				Program = program,
				Year = year,
				TotalBatches = count
			});
		}

		// =====================================================
		// 9️⃣ INSTRUCTOR → VIEW STUDENTS IN A BATCH
		// =====================================================
		[HttpGet("batch/{batchId}/students")]
		public IActionResult GetStudentsInBatch(string batchId)
		{
			return Ok(_context.StudentBatchAssignments
				.Where(s => s.BatchId == batchId)
				.Select(s => new
				{
					s.Student.StudentId,
					s.Student.StudentName,
					s.Student.StudentEmail
				}).ToList());
		}
		[HttpPost("assign-single-batch")]
		public IActionResult AssignSingleBatch([FromBody] AutoAssignBatchDTO dto)
		{
			// 1️⃣ Get ONLY unassigned students (YEAR SAFE)
			var students = _context.Student
				.Where(s =>
					s.StudentQualification == dto.Qualification &&
					s.StudentProgram == dto.Program &&
					s.Year == dto.Year &&                                  // ✅ YEAR FILTER
					!_context.StudentBatchAssignments.Any(a =>
						a.StudentId == s.StudentId &&
						a.Batches.CourseId == dto.CourseId))
				.OrderBy(s => s.StudentId)
				.ToList();

			if (!students.Any())
				return BadRequest("No students left to assign");

			// 2️⃣ Create ONE new batch
			int batchCount = _context.CourseBatches.Count() + 1;
			string batchId = $"B{batchCount:D3}";

			var batch = new CourseBatch
			{
				BatchId = batchId,
				CourseId = dto.CourseId,
				InstructorId = dto.InstructorId,
				MaxStudents = dto.BatchSize,
				CurrentStudents = 0,
				IsActive = true
			};

			_context.CourseBatches.Add(batch);
			_context.SaveChanges();

			// 3️⃣ Assign ONLY BatchSize students
			var batchStudents = students.Take(dto.BatchSize).ToList();

			foreach (var student in batchStudents)
			{
				_context.StudentBatchAssignments.Add(new StudentBatchAssignment
				{
					BatchId = batchId,
					StudentId = student.StudentId
				});

				batch.CurrentStudents++;
			}

			batch.IsActive = batch.CurrentStudents < batch.MaxStudents;
			_context.SaveChanges();

			int remaining = students.Count - batchStudents.Count;

			// 4️⃣ Response for UI / Swagger
			return Ok(new
			{
				Message = "Single batch assigned successfully",
				BatchId = batchId,
				AssignedStudents = batchStudents.Count,
				RemainingStudents = remaining,
				NextStep = remaining > 0
					? "Assign next batch to same or another instructor"
					: "All students assigned"
			});
		}

		// =====================================================
		// 🔟 INSTRUCTOR FULL DASHBOARD
		// =====================================================
		[HttpGet("instructor/{instructorId}/dashboard")]
		public IActionResult InstructorDashboard(string instructorId)
		{
			var data = _context.CourseBatches
				.Where(b => b.InstructorId == instructorId)
				.Select(b => new
				{
					b.BatchId,
					Course = new
					{
						b.Course.CourseId,
						b.Course.CourseName
					},
					Students = _context.StudentBatchAssignments
						.Where(s => s.BatchId == b.BatchId)
						.Select(s => new
						{
							s.Student.StudentId,
							s.Student.StudentName
						}).ToList()
				}).ToList();

			return Ok(data);
		}

	}
}








