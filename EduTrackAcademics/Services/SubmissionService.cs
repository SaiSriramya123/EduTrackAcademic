using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using NuGet.Protocol.Core.Types;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exception;

namespace EduTrackAcademics.Services
{
	public class SubmissionService: ISubmissionService
	{
		private readonly ISubmissionRepository _repo;

		public SubmissionService(ISubmissionRepository repo)
		{
			_repo = repo;
		}

		public async Task<List<ViewAssessmentDto>> GetStudentAssessmentsAsync(string studentId)
		{
			bool isEnrolled = await _repo.IsStudentEnrolledAsync(studentId);

			if (!isEnrolled)
			{
				throw new InvalidOperationException("Student is not enrolled in any courses");
			}
			return await _repo.GetAssessmentsByStudentIdAsync(studentId);
		}

		public async Task<List<StartAssessmentDto>> StartAssessmentAsync(string studentId, string assessmentId)
		{
			// Get enrolled courses
			var courseIds = await
				_repo.GetStudentCourseIdsAsync(studentId);

			if (courseIds == null || !courseIds.Any())
				throw new ApplicationException("Student not enrolled in any courses");

			// Get assessment details
			var assessment = await
				_repo.GetAssessmentByIdAsync(assessmentId);

			if (assessment == null)
				throw new ApplicationException("Assessment not found");

			//Check if assessment belongs to enrolled course
			if (!courseIds.Contains(assessment.CourseID))
				throw new ApplicationException("Student not authorized for this assessment");

			// Validate status and due date
			if (assessment.Status != "Open" && assessment.DueDate < DateTime.Now)
				throw new AssessmentInactiveException("Assessment is not active");

			// Load questions
			var questions = await
				_repo.GetQuestionsByAssessmentIdAsync(assessmentId);

			if (!questions.Any())
				throw new ApplicationException("No questions available");

			return questions;
		}
	}
}
