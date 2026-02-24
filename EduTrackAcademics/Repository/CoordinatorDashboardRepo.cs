using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Repository
{
	public class CoordinatorDashboardRepo : ICoordinatorDashboardRepo
	{
		private readonly EduTrackAcademicsContext _context;

		public CoordinatorDashboardRepo(EduTrackAcademicsContext context)
		{
			_context = context;
		}

		public IEnumerable<object> GetPrograms() =>
			_context.Programs.Select(p => new { p.ProgramId, p.ProgramName, p.QualificationId }).ToList();

		public IEnumerable<object> GetAcademicYears(string programId) =>
			_context.AcademicYear.Where(y => y.ProgramId == programId)
				.Select(y => new { y.AcademicYearId, y.YearNumber }).ToList();

		public Course AddCourse(CourseDTO dto)
		{
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
			return course;
		}

		public IEnumerable<object> GetCourses(string yearId) =>
			_context.Course.Where(c => c.AcademicYearId == yearId)
				.Select(c => new { c.CourseId, c.CourseName, c.Credits, c.DurationInWeeks }).ToList();

		public IEnumerable<object> GetStudents(string qualification, string program, int year) =>
			_context.Student.Where(s => s.StudentQualification == qualification &&
										s.StudentProgram == program &&
										s.Year == year)
				.Select(s => new { s.StudentId, s.StudentName, s.StudentEmail }).ToList();

		public IEnumerable<object> GetInstructors(string skill) =>
			_context.Instructor.Where(i => i.InstructorSkills.Contains(skill))
				.Select(i => new { i.InstructorId, i.InstructorName, i.InstructorSkills }).ToList();

		public IEnumerable<object> GetBatches(string program, int year) =>
			_context.CourseBatches.Include(b => b.Course)
				.Where(b => _context.AcademicYear.Any(y =>
					y.AcademicYearId == b.Course.AcademicYearId &&
					y.YearNumber == year &&
					y.Program.ProgramName == program))
				.Select(b => new { b.BatchId, b.Course.CourseName, b.InstructorId, b.CurrentStudents, b.MaxStudents, b.IsActive }).ToList();

		public int GetBatchCount(string program, int year) =>
			_context.CourseBatches.Include(b => b.Course)
				.Count(b => _context.AcademicYear.Any(y =>
					y.AcademicYearId == b.Course.AcademicYearId &&
					y.YearNumber == year &&
					y.Program.ProgramName == program));

		public IEnumerable<object> GetStudentsInBatch(string batchId) =>
			_context.StudentBatchAssignments.Where(s => s.BatchId == batchId)
				.Select(s => new { s.Student.StudentId, s.Student.StudentName, s.Student.StudentEmail }).ToList();

		public object AssignBatches(AutoAssignBatchDTO dto)
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
				return new { Message = "No students found" };

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

			return new
			{
				Message = "Batch assignment completed",
				AssignedStudents = assigned
			};
		}

		public object AssignSingleBatch(AutoAssignBatchDTO dto)
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
				return new { Message = "No students left to assign" };

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

			return new
			{
				Message = "Single batch assigned successfully",
				BatchId = batchId,
				AssignedStudents = batchStudents.Count,
				RemainingStudents = remaining,
				NextStep = remaining > 0
					? "Assign next batch to same or another instructor"
					: "All students assigned"
			};
		}

		public IEnumerable<object> GetInstructorBatches(string instructorId) =>
			_context.CourseBatches.Where(b => b.InstructorId == instructorId)
				.Select(b => new { b.BatchId, b.Course.CourseName, b.MaxStudents, b.CurrentStudents, b.IsActive }).ToList();

		public IEnumerable<object> InstructorDashboard(string instructorId) =>
			_context.CourseBatches.Where(b => b.InstructorId == instructorId)
				.Select(b => new
				{
					b.BatchId,
					Course = new { b.Course.CourseId, b.Course.CourseName },
					Students = _context.StudentBatchAssignments.Where(s => s.BatchId == b.BatchId)
						.Select(s => new { s.Student.StudentId, s.Student.StudentName }).ToList()
				}).ToList();
	}
}
