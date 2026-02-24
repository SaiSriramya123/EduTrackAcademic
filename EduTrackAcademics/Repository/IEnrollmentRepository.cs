using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
	public interface IEnrollmentRepository
	{
		Task<int> AddEnrollmentAsync(Enrollment enrollment);
		Task<bool> CheckIdExistsAsync(string enrollmentId);
		Task<int> GetEnrollmentCountAsync();
		Task<bool> IsEnrolledAsync(string studentId, string courseId);
		Task<List<Module>> GetModulesByCourseAsync(string courseId);
		Task<double> GetCourseProgressPercentageAsync(string studentId, string courseId);
		Task UpdateEnrollmentStatusAsync(string studentId, string courseId, string status);
	}
}
