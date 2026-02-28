using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exception;
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
			var enrollments = _context.Enrollment
				.Include(e => e.Student)
				.Where(e =>
					e.CourseId == dto.CourseId &&
					e.Status == "Active" &&
					e.Student.StudentQualification == dto.Qualification &&
					e.Student.StudentProgram == dto.Program &&
					e.Student.Year == dto.Year)
				.Where(e =>
					!_context.StudentBatchAssignments.Any(a =>
						a.StudentId == e.StudentId))
				.OrderBy(e => e.Student.StudentId)
				.ToList();

			if (!enrollments.Any())
				return new { Message = "No enrolled students available" };

			int batchCounter = _context.CourseBatches.Count() + 1;
			int assigned = 0;

			for (int i = 0; i < enrollments.Count; i += dto.BatchSize)
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

				var group = enrollments.Skip(i).Take(dto.BatchSize).ToList();

				foreach (var enrollment in group)
				{
					_context.StudentBatchAssignments.Add(new StudentBatchAssignment
					{
						StudentId = enrollment.StudentId,
						BatchId = batchId
					});

					enrollment.Status = "Assigned";
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
			// 1️⃣ Get ONLY enrolled & unassigned students
			var students = _context.Enrollment
				.Include(e => e.Student)
				.Where(e =>
					e.CourseId == dto.CourseId &&
					e.Status == "Active" &&
					e.Student.StudentQualification == dto.Qualification &&
					e.Student.StudentProgram == dto.Program &&
					e.Student.Year == dto.Year)
				.Where(e =>
					!_context.StudentBatchAssignments.Any(a =>
						a.StudentId == e.StudentId))
				.OrderBy(e => e.Student.StudentId)
				.ToList();

			if (!students.Any())
				return new { Message = "No enrolled students left to assign" };

			// 2️⃣ Create batch
			string batchId = $"B{_context.CourseBatches.Count() + 1:D3}";

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

			// 3️⃣ Assign students to batch
			var batchStudents = students.Take(dto.BatchSize).ToList();

			foreach (var enrollment in batchStudents)
			{
				_context.StudentBatchAssignments.Add(new StudentBatchAssignment
				{
					StudentId = enrollment.StudentId,
					BatchId = batchId
				});

				enrollment.Status = "Assigned"; // ✅ important
				batch.CurrentStudents++;
			}

			batch.IsActive = batch.CurrentStudents < batch.MaxStudents;
			_context.SaveChanges();

			return new
			{
				Message = "Batch created & students assigned",
				BatchId = batchId,
				AssignedStudents = batchStudents.Count,
				RemainingStudents = students.Count - batchStudents.Count
			};
		}
		public object UpdateCourse(string courseId, CourseDTO dto)
		{
			// Find course by CourseId
			var course = _context.Course.FirstOrDefault(c => c.CourseId == courseId);
			if (course == null)
				throw new CourseNotFoundException(courseId);

			// Update properties from DTO
			course.CourseName = dto.CourseName;
			course.AcademicYearId = dto.AcademicYearId; // map year
			course.Credits = dto.Credits;               // optional
			course.DurationInWeeks = dto.DurationInWeeks; // optional

			_context.SaveChanges(); // save changes to DB

			return course;
		}

		public bool DeleteCourse(string courseId)
		{
			var course = _context.Course.FirstOrDefault(c => c.CourseId == courseId);
			if (course == null)
				throw new CourseNotFoundException(courseId);

			_context.Course.Remove(course);
			_context.SaveChanges(); // commit delete

			return true; // indicate success
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
