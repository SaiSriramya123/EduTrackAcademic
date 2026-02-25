using System.Runtime.Intrinsics.Arm;
using EduTrackAcademics.Data;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
	public class StudentProgressesRepository: IStudentProgressesRepository
	{
		private readonly EduTrackAcademicsContext _context;

		public StudentProgressesRepository(EduTrackAcademicsContext context)
		{
			_context = context;
		}

		public async Task<int> AddProgressRecordAsync(StudentProgress progress)
		{
			_context.StudentProgress.Add(progress);
			return await Task.FromResult(1);
		}

		
		public async Task<bool> CheckIfProgressExistsAsync(string studentId, string contentId)
		{
			return await Task.FromResult(
				_context.StudentProgress.Any(p =>
					p.StudentId == studentId &&
					p.ContentId == contentId)
			);
		}

		public async Task<int> GetProgressCountAsync()
		{
			return await Task.FromResult<int>(_context.StudentProgress.Count());
		}
	}
}
