using EduTrackAcademics.DTO;
using EduTrackAcademics.Exception;

using EduTrackAcademics.Repository;


namespace EduTrackAcademics.Service
{
	public class CoordinatorDashboardService : ICoordinatorDashboardService
	{
		private readonly ICoordinatorDashboardRepo _repo;

		public CoordinatorDashboardService(ICoordinatorDashboardRepo repo)
		{
			_repo = repo;
		}

		public IEnumerable<object> GetPrograms()
		{
			var data = _repo.GetPrograms();
			if (!data.Any())
				throw new CourseNotFoundException("Programs");

			return data;
		}

		public IEnumerable<object> GetAcademicYears(string programId)
		{
			return _repo.GetAcademicYears(programId);
		}

		public object AddCourse(CourseDTO dto)
		{
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));

			return _repo.AddCourse(dto);
		}

		public IEnumerable<object> GetCourses(string yearId)
		{
			var courses = _repo.GetCourses(yearId);
			if (!courses.Any())
				throw new CourseNotFoundException(yearId);

			return courses;
		}

		public IEnumerable<object> GetStudents(string qualification, string program, int year)
		{
			var students = _repo.GetStudents(qualification, program, year);
			if (!students.Any())
				throw new StudentNotFoundException($"{qualification}-{program}-{year}");

			return students;
		}

		public IEnumerable<object> GetInstructors(string skill)
		{
			var instructors = _repo.GetInstructors(skill);
			if (!instructors.Any())
				throw new InstructorNotFoundException(skill);

			return instructors;
		}

		public IEnumerable<object> GetBatches(string program, int year)
		{
			return _repo.GetBatches(program, year);
		}

		public object GetBatchCount(string program, int year)
		{
			return _repo.GetBatchCount(program, year);
		}

		public IEnumerable<object> GetStudentsInBatch(string batchId)
		{
			var students = _repo.GetStudentsInBatch(batchId);
			if (!students.Any())
				throw new BatchNotFoundException(batchId);

			return students;
		}

		public object AssignBatches(AutoAssignBatchDTO dto)
		{
			return _repo.AssignBatches(dto);
		}

		public object AssignSingleBatch(AutoAssignBatchDTO dto)
		{
			return _repo.AssignSingleBatch(dto);
		}
		public object UpdateCourse(string courseId, CourseDTO dto)
		{
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));

			return _repo.UpdateCourse(courseId, dto);
		}

		public bool DeleteCourse(string courseId)
		{
			if (string.IsNullOrEmpty(courseId))
				throw new ArgumentException("Course ID is required");

			return _repo.DeleteCourse(courseId);
		}

		public IEnumerable<object> GetInstructorBatches(string instructorId)
		{
			return _repo.GetInstructorBatches(instructorId);
		}

		public IEnumerable<object> InstructorDashboard(string instructorId)
		{
			return _repo.InstructorDashboard(instructorId);
		}
	}
}
