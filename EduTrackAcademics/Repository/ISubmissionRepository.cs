using EduTrackAcademics.Model;
using EduTrackAcademics.DTO;

namespace EduTrackAcademics.Repository
{
	public interface ISubmissionRepository
	{

		Task<bool> IsStudentEnrolledAsync(string studentId);

		Task<List<ViewAssessmentDto>> GetAssessmentsByStudentIdAsync(string studentId);

		Task<ViewAssessmentDto> GetAssessmentByIdAsync(string assessmentId);

		Task<List<string>> GetStudentCourseIdsAsync(string studentId);

		Task<List<StartAssessmentDto>> GetQuestionsByAssessmentIdAsync(string assessmentId);
	}
}
