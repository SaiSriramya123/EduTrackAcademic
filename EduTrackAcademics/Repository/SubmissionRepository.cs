using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
	public class SubmissionRepository : ISubmissionRepository
	{
		private readonly EduTrackAcademicsContext _context;

		public SubmissionRepository(EduTrackAcademicsContext context)
		{
			_context = context;
		}

		public async Task<bool> IsStudentEnrolledAsync(string studentId)
		{
			return await _context.Enrollment
				.AnyAsync(e => e.StudentId == studentId);
		}

		public async Task<List<ViewAssessmentDto>> GetAssessmentsByStudentIdAsync(string studentId)
		{
			//var result = await (
			//	from e in _context.Enrollment
			//	join a in _context.Assessments
			//		on e.CourseId equals a.CourseID
			//	join c in _context.Course
			//		on a.CourseID equals c.CourseId
			//	where e.StudentId == studentId
			//	select a).ToListAsync();

			var enrolledCourseIds = await _context.Enrollment
			.Where(e => e.StudentId == studentId)
			.Select(e => e.CourseId)
			.ToListAsync();

			var assessments = await _context.Assessments
				.Where(a => enrolledCourseIds.Contains(a.CourseID))
				.Include(a => a.Course)
				.Select(a => new ViewAssessmentDto
				{
					CourseName = a.Course.CourseName,
					//AssessmentID = a.AssessmentID,
					Type = a.Type,
					MaxMarks = a.MaxMarks,
					DueDate = a.DueDate,
					Status = a.Status
				})
				.ToListAsync();

			return assessments;
		}

		public async Task<ViewAssessmentDto> GetAssessmentByIdAsync(string assessmentId)
		{
			return await _context.Assessments
				.Where(a => a.AssessmentID == assessmentId)
				.Select(a => new ViewAssessmentDto
				{
					AssessmentID = a.AssessmentID,
					CourseID = a.CourseID,
					Type = a.Type,
					MaxMarks = a.MaxMarks,
					DueDate = a.DueDate,
					Status = a.Status
				})
				.FirstOrDefaultAsync();
		}

		public async Task<List<string>> GetStudentCourseIdsAsync(string studentId)
		{
			return await _context.Enrollment
				.Where(e => e.StudentId == studentId)
				.Select(e => e.CourseId)
				.ToListAsync();
		}

		public async Task<List<StartAssessmentDto>> GetQuestionsByAssessmentIdAsync(string assessmentId)
		{
			return await _context.Questions
				.Where(q => q.AssessmentId == assessmentId)
				.OrderBy(q => q.OrderNo)
				.Select(q => new StartAssessmentDto
				{
					AssessmentId = q.AssessmentId,
					QuestionId = q.QuestionId,
					QuestionType = q.QuestionType,
					QuestionText = q.QuestionText,
					OptionA = q.OptionA,
					OptionB = q.OptionB,
					OptionC = q.OptionC,
					OptionD = q.OptionD,
					Marks = q.Marks,
					OrderNo = q.OrderNo
				})
				.ToListAsync();
		}
	}
}
