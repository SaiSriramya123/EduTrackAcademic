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

		public async Task<string> AddEnrollmentAsync(EnrollmentDto dto)
		{
			// Generate unique EnrollmentId
			int count = await _repo.GetEnrollmentCountAsync();
			string enrollmentId = $"E{(count + 1):D3}";

			if (await _repo.CheckIdExistsAsync(enrollmentId))
				throw new EnrollmentAlreadyExistsException($"Enrollment already exists");

			// Fetch course credits from DB
			var course = await _context.Course.FindAsync(dto.CourseId);
			if (course == null)
				throw new ApplicationException("Course not found");

			var enrollment = new Enrollment
			{
				EnrollmentId = enrollmentId,
				StudentId = dto.StudentId,
				CourseId = dto.CourseId,
				EnrollmentDate = DateTime.Now,   // ✅ auto-generated
				Status = "Active",
				Credits = course.Credits         // ✅ taken from Course table
			};

			 await _repo.AddEnrollmentAsync(enrollment);
			return  enrollmentId;

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
