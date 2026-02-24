using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
	public interface IInstructorRepo
	{
		List<CourseBatch> GetBatches(string instructorId);
		List<StudentBatchAssignment> GetStudents(string batchId);
		List<Module> GetModules(string courseId);
		List<Content> GetContent(string moduleId);
		List<Assessment> GetAssessments(string courseId);
		List<Question> GetQuestions(string assessmentId);
		void SaveAttendance(Attendance attendance);
		List<Attendance> GetAttendance(string batchId);
		void Evaluate(string assessmentId, int marks, string feedback);
	}
}
