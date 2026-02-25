using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
	public interface IStudentProgressesRepository
	{
		Task<int> AddProgressRecordAsync(StudentProgress progress);

		Task<bool> CheckIfProgressExistsAsync(string studentId, string contentId);

		Task<int> GetProgressCountAsync();
	}
}
