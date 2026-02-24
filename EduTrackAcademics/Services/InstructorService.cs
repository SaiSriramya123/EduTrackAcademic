using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;

namespace EduTrackAcademics.Services
{
	public class InstructorService : IInstructorService
	{
		private readonly IInstructorRepo _repo;

		public InstructorService(IInstructorRepo repo)
		{
			_repo = repo;
		}

		public List<CourseBatch> GetBatches(string instructorId)
			=> _repo.GetBatches(instructorId);

		public List<StudentBatchAssignment> GetStudents(string batchId)
			=> _repo.GetStudents(batchId);

		public List<Module> GetModules(string courseId)
			=> _repo.GetModules(courseId);

		public List<Content> GetContent(string moduleId)
			=> _repo.GetContent(moduleId);

		public List<Assessment> GetAssessments(string courseId)
			=> _repo.GetAssessments(courseId);

		public List<Question> GetQuestions(string assessmentId)
			=> _repo.GetQuestions(assessmentId);

		public void Evaluate(string assessmentId, int marks, string feedback)
			=> _repo.Evaluate(assessmentId, marks, feedback);

		public void MarkAttendance(Attendance attendance)
			=> _repo.SaveAttendance(attendance);

		public List<Attendance> GetAttendance(string batchId)
			=> _repo.GetAttendance(batchId);
	}
}
