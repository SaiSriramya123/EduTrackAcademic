using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using EduTrackAcademics.Exception;

namespace EduTrackAcademics.Services
{
	public class EnrollmentService : IEnrollmentService
	{
		private readonly IEnrollmentRepository _repo;
		private readonly EduTrackAcademicsContext _context;

		public EnrollmentService(IEnrollmentRepository repo, EduTrackAcademicsContext context)
		{
			_repo = repo;
			_context = context;
		}

		public async Task<int> AddEnrollmentAsync(EnrollmentDto dto)
		{
			int count = await _repo.GetEnrollmentCountAsync();
			string enrollmentId = $"E{(count + 1):D3}";

			if (await _repo.CheckIdExistsAsync(enrollmentId))
				throw new EnrollmentAlreadyExistsException($"Enrollment already exists");

			var enrollment = new Enrollment
			{
				EnrollmentId = enrollmentId,
				StudentId = dto.StudentId,
				CourseId = dto.CourseId,
				EnrollmentDate = DateTime.Now,
				Status = "Active"
			};

			return await _repo.AddEnrollmentAsync(enrollment);

		}

		public async Task<List<Module>> GetContentForStudentAsync(string studentId, string courseId)
		{
			if (!await _repo.IsEnrolledAsync(studentId, courseId))
				throw new EnrollmentNotExistsException("You must enroll first", 403);

			return await _repo.GetModulesByCourseAsync(courseId);
		}

		public async Task<double> GetCourseProgressPercentageAsync(string studentId, string courseId)
		{
			return await _repo.GetCourseProgressPercentageAsync(studentId, courseId);
		}

		public async Task<bool> ProcessCourseCompletionAsync(string studentId, string courseId)
		{
			double progress = await GetCourseProgressPercentageAsync(studentId, courseId);

			if (progress >= 100)
			{
				await _repo.UpdateEnrollmentStatusAsync(studentId, courseId, "Completed");
				return true;
			}

			return false;
		}
	}

}
