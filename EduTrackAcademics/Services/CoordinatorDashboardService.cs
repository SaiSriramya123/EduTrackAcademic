using EduTrackAcademics.DTO;
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

		public IEnumerable<object> GetPrograms() => _repo.GetPrograms();
		public IEnumerable<object> GetAcademicYears(string programId) => _repo.GetAcademicYears(programId);
		public object AddCourse(CourseDTO dto) => _repo.AddCourse(dto);
		public IEnumerable<object> GetCourses(string yearId) => _repo.GetCourses(yearId);
		public IEnumerable<object> GetStudents(string qualification, string program, int year) => _repo.GetStudents(qualification, program, year);
		public IEnumerable<object> GetInstructors(string skill) => _repo.GetInstructors(skill);
		public IEnumerable<object> GetBatches(string program, int year) => _repo.GetBatches(program, year);
		public object GetBatchCount(string program, int year) => _repo.GetBatchCount(program, year);
		public IEnumerable<object> GetStudentsInBatch(string batchId) => _repo.GetStudentsInBatch(batchId);
		public object AssignBatches(AutoAssignBatchDTO dto) => _repo.AssignBatches(dto);
		public object AssignSingleBatch(AutoAssignBatchDTO dto) => _repo.AssignSingleBatch(dto);
		public IEnumerable<object> GetInstructorBatches(string instructorId) => _repo.GetInstructorBatches(instructorId);
		public IEnumerable<object> InstructorDashboard(string instructorId) => _repo.InstructorDashboard(instructorId);
	}
}
