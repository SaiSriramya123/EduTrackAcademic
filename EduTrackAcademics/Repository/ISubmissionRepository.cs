using EduTrackAcademics.Model;
using EduTrackAcademics.DTO;

namespace EduTrackAcademics.Repository
{
	public interface ISubmissionRepository
	{

		Task<bool> IsStudentEnrolledAsync(string studentId);

		Task<List<ViewAssessmentDto>> GetAssessmentsByStudentIdAsync(string studentId);

		Task<ViewAssessmentDto> GetAssessmentByIdAsync(string assessmentId);
		Task<int> GetSubmissionCountAsync();

		Task<List<string>> GetStudentCourseIdsAsync(string studentId);

		Task<List<StartAssessmentDto>> GetQuestionsByAssessmentIdAsync(string assessmentId);

		Task InsertOrUpdateAnswerAsync(StudentAnswerDto dto);

		Task<string> SubmitAssessmentAsync(Submission submission);

		Task<(int score, double percentage)> CalculateScoreAsync(string studentId, string assessmentId);

		Task UpdateSubmissionAsync(string submissionId, int score, string feedback);
	}
}
