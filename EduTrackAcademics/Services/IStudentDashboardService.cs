using EduTrackAcademics.DTO;
namespace EduTrackAcademics.Services
{
	public interface IStudentDashboardService
	{
		Task<StudentDTO> GetStudentDetails(string studentId);
		Task<int> GetCreditPointsAsync(string studentId);
		Task<(DateTime DueDate, string Type, string CourseName)> GetAssignmentDetailsForStudentAsync(string studentId, string courseId);

		Task<AuditLogDTO> GetAuditLogAsync(string studentId);
	}
}
