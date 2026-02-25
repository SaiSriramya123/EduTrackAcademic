using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using EduTrackAcademics.DTO;

namespace EduTrackAcademics.Services
{
	public interface ISubmissionService
	{
		Task<List<ViewAssessmentDto>> GetStudentAssessmentsAsync(string studentId);
		Task<List<StartAssessmentDto>> StartAssessmentAsync(string studentId, string assessmentId);
	}
}
