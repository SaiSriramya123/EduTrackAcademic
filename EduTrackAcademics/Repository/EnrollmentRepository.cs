using System.Runtime.Intrinsics.Arm;
using EduTrackAcademics.Data;
using EduTrackAcademics.Model;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Repository
{
	public class EnrollmentRepository: IEnrollmentRepository
	{
		private readonly EduTrackAcademicsContext _context;

		public EnrollmentRepository(EduTrackAcademicsContext context)
		{
			_context = context;
		}
		public async Task<int> GetEnrollmentCountAsync()
		{
			return await _context.Enrollment.CountAsync();
		}

		public async Task<bool> CheckIdExistsAsync(string enrollmentId)
		{
			return await Task.FromResult(
				_context.Enrollment.Any(e => e.EnrollmentId == enrollmentId)
			);
		}

		public async Task<int> AddEnrollmentAsync(Enrollment enrollment)
		{
			_context.Enrollment.Add(enrollment);
			await _context.SaveChangesAsync();  
			return 1;
		}


		public async Task<bool> IsEnrolledAsync(string studentId, string courseId)
		{
			return await Task.FromResult(
				_context.Enrollment.Any(e =>
					e.StudentId == studentId && e.CourseId == courseId)
			);
		}

		public async Task<List<Module>> GetModulesByCourseAsync(string courseId)
		{
			var modules = _context.Modules
				.Where(m => m.CourseID == courseId)
				.OrderBy(m => m.SequenceOrder)
				.ToList();

			// Attach contents to each module
			foreach (var module in modules)
			{
				module.Content = _context.Contents
					.Where(c => c.ModuleID == module.ModuleID)
					.ToList();
			}

			return await Task.FromResult(modules);
		}

		
		public async Task<double> GetCourseProgressPercentageAsync(string studentId, string courseId)
		{
			var moduleIds = _context.Modules
				.Where(m => m.CourseID == courseId)
				.Select(m => m.ModuleID)
				.ToList();

			var contentIds = _context.Contents
				.Where(c => moduleIds.Contains(c.ModuleID))
				.Select(c => c.ContentID)
				.ToList();

			int totalContentCount = contentIds.Count;

			if (totalContentCount == 0)
				return 0;

			int completedItems = _context.StudentProgress
				.Count(p =>
					p.StudentId == studentId &&
					contentIds.Contains(p.ContentId) &&
					p.IsCompleted);

			double percentage = ((double)completedItems / totalContentCount) * 100;

			return await Task.FromResult(Math.Round(percentage, 2));
		}

		
		public async Task UpdateEnrollmentStatusAsync(string studentId, string courseId, string status)
		{
			var enrollment = _context.Enrollment
				.FirstOrDefault(e =>
					e.StudentId == studentId &&
					e.CourseId == courseId);

			if (enrollment != null)
			{
				enrollment.Status = status;
			}

			await Task.CompletedTask;
		}
	}
}
