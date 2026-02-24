using EduTrackAcademics.DTO;

namespace EduTrackAcademics.Repository
{
	public interface IStudentDashboardRepo
	{
		Task<bool> StudentExistsAsync(string studentId);
		Task<StudentDashboardDTO?> GetStudentDetailsAsync(string studentId);
		Task<int> GetCreditPointsAsync(string studentId);
		Task<(DateTime DueDate, string Type, string CourseName)?> GetAssignmentDetailsAsync(string courseId);
		Task<bool> IsStudentEnrolledInCourseAsync(string studentId, string courseId);
		Task<AuditLogDTO?> GetStudentLoginHistoryAsync(string studentId);
	}
}
