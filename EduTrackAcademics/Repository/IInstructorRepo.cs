using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
	public interface IInstructorRepo
	{

		// Module
		Task<string> GenerateModuleIdAsync();
		Task AddModuleAsync(Module module);
		Task<List<Module>> GetModulesByCourseAsync(string courseId);
		Task<Module> GetModuleByIdAsync(string moduleId);
		Task UpdateModuleAsync(Module module);

		// Content
		Task<string> GenerateContentIdAsync();
		Task<bool> ModuleExistsAsync(string moduleId);
		Task AddContentAsync(Content content);
		Task<Content> GetContentByIdAsync(string contentId);
		Task<List<Content>> GetContentByModuleAsync(string moduleId);
		Task UpdateContentAsync(Content content);
		Task DeleteContentAsync(Content content);

		// ASSESSMENT
		Task<string> GenerateAssessmentIdAsync();
		Task AddAssessmentAsync(Assessment assessment);
		Task<Assessment?> GetAssessmentByIdAsync(string assessmentId);
		Task<List<Assessment>> GetAssessmentsByCourseAsync(string courseId);
		Task UpdateAssessmentAsync(Assessment assessment);
		Task DeleteAssessmentAsync(Assessment assessment);

		// QUESTIONS
		Task<string> GenerateQuestionIdAsync();
		Task<bool> QuestionExistsAsync(string questionId);
		Task AddQuestionAsync(Question question);
		Task<Question?> GetQuestionByIdAsync(string questionId);
		Task<List<Question>> GetQuestionsByAssessmentAsync(string assessmentId);
		Task UpdateQuestionAsync(Question question);
		Task DeleteQuestionAsync(Question question);

		// ATTENDANCE
		Task<string> GenerateAttendanceIdAsync();
		Task<bool> AttendanceExistsAsync(string enrollmentId, DateTime date);
		Task AddAttendanceAsync(Attendance attendance);
		Task UpdateAttendanceAsync(Attendance attendance);
		Task<Attendance?> GetAttendanceByIdAsync(string attendanceId);
		Task<List<Attendance>> GetAttendanceByBatchAsync(string batchId);
		Task<List<Attendance>> GetAttendanceByDateAsync(DateTime date);
		Task<List<Attendance>> GetAttendanceByEnrollmentAsync(string enrollmentId);

		Task CommitAsync();

	}
}
