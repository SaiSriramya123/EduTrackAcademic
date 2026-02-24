using EduTrackAcademics.Model;

namespace EduTrackAcademics.Services
{
	public interface IInstructorService
	{
		List<CourseBatch> GetBatches(string instructorId);
		List<StudentBatchAssignment> GetStudents(string batchId);
		List<Module> GetModules(string courseId);
		List<Content> GetContent(string moduleId);
		List<Assessment> GetAssessments(string courseId);
		List<Question> GetQuestions(string assessmentId);
		void Evaluate(string assessmentId, int marks, string feedback);
		void MarkAttendance(Attendance attendance);
		List<Attendance> GetAttendance(string batchId);
	}
}
