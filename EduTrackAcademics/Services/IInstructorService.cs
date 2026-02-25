using EduTrackAcademics.Model;

namespace EduTrackAcademics.Services
{
	public interface IInstructorService
	{
		Task<object> GetBatches(string instructorId);
		Task<object> GetStudents(string batchId);
		Task<object> GetDashboard(string instructorId);

		Task AddModule(Module module);
		Task UpdateModule(Module module);
		Task DeleteModule(string id);
		Task<object> GetModules(string courseId);
		Task<bool> CompleteModule(string moduleId);

		Task AddContent(Content content);
		Task UpdateContent(Content content);
		Task DeleteContent(string id);
		Task<object> GetContent(string moduleId);

		Task AddAssessment(Assessment a);
		Task UpdateAssessment(Assessment a);
		Task DeleteAssessment(string id);
		Task<object> GetAssessments(string courseId);
		Task<object> GetQuestions(string assessmentId);
		Task EvaluateAssessment(string id, int marks, string feedback);

		Task MarkAttendance(Attendance attendance);
		Task UpdateAttendance(string id, Attendance updated);
		Task DeleteAttendance(string id, string reason);
		Task<object> GetAttendance(string batchId);
		Task<object> GetAttendanceReport(string batchId);
		Task<object> GetIrregularStudents(string batchId);
	}
}
