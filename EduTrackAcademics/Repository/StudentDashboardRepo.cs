using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Dummy;
using Humanizer;
using Microsoft.EntityFrameworkCore;
namespace EduTrackAcademics.Repository
{
	public class StudentDashboardRepo : IStudentDashboardRepo
	{
		private readonly EduTrackAcademicsContext _context;
		public StudentDashboardRepo(EduTrackAcademicsContext context)
		{
			_context = context;
		}
		public async Task<bool> StudentExistsAsync(string studentId)
		{
			return await _context.Student.
				AnyAsync(s => s.StudentId == studentId);
		}
		public async Task<StudentDashboardDTO?> GetStudentDetailsAsync(string studentId)
		{
			return await _context.Student
				.Where(s => s.StudentId == studentId)
				.Select(s => new StudentDashboardDTO
				{
					StudentId = s.StudentId,
					StudentDetails = new StudentDTO
					{
						StudentName = s.StudentName,
						StudentQualification = s.StudentQualification
					}
				})
				.AsNoTracking()
				.FirstOrDefaultAsync();
		}
		 
		// Get total credits for completed enrollments
		  public async Task<int> GetCreditPointsAsync(string studentId) 
		  { 
			return await _context.Enrollment
				.Where(e => e.StudentId == studentId && e.Status == "Completed") 
				.Join(_context.Course, e => e.CourseId, c => c.CourseId, (e, c)=> c.Credits) 
				.SumAsync(); 
		}

		// to get assignment due
		public async Task<bool> IsStudentEnrolledInCourseAsync(string studentId, string courseId)
		{ 
			return await _context.Enrollment.
				AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId); 
		}

		public async Task<(DateTime DueDate, string Type, string CourseName)?> GetAssignmentDetailsAsync(string courseId) 
		{ 
			var result = await _context.Assessments
				.Where(a => a.CourseID == courseId && a.Type == "Assignment")
				.Join(_context.Course, a => a.CourseID, c => c.CourseId, (a, c) 
				=> new { a.DueDate, a.Type, c.CourseName 
				})
				.FirstOrDefaultAsync(); 
			
			if (result == null) 
				return null;
			return (result.DueDate, result.Type, result.CourseName);
		}


		//audit details
		public async Task<AuditLogDTO?> GetStudentLoginHistoryAsync(string studentId)
		{

			var history = await _context.AuditLog
				.Where(h => h.StudentId == studentId)
				.OrderByDescending(h => h.LoginTime)
				.FirstOrDefaultAsync();

			if (history == null)
				return null;

			// Ensure LogoutTime is non-nullable for the DTO; fallback to LoginTime if missing
			var logout = history.LogoutTime ?? history.LoginTime;

			return new AuditLogDTO
			{
				Id = history.Id,
				StudentId = history.StudentId,           // keep string type
				LoginTime = history.LoginTime,
				LogoutTime = logout,
				TimeSpent = logout - history.LoginTime
			};
		}
	}
}