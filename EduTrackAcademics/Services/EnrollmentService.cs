using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using EduTrackAcademics.Exception;
using Microsoft.EntityFrameworkCore;

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
			// 1️⃣ Validate course (course already mapped to year)
			var course = await _context.Course.FindAsync(dto.CourseId);
			if (course == null)
				throw new ApplicationException("Course not found");

			// 2️⃣ Prevent duplicate enrollment (Student + Course)
			bool alreadyEnrolled = await _context.Enrollment.AnyAsync(e =>
				e.StudentId == dto.StudentId &&
				e.CourseId == dto.CourseId &&
				e.Status == "Active");

			if (alreadyEnrolled)
				throw new EnrollmentAlreadyExistsException("Student already enrolled for this course");

			// 3️⃣ Generate SAFE unique EnrollmentId
			string enrollmentId = "E" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

			// 4️⃣ Create enrollment (NO batch here)
			var enrollment = new Enrollment
			{
				EnrollmentId = enrollmentId,
				StudentId = dto.StudentId,
				CourseId = dto.CourseId,
				EnrollmentDate = DateTime.Now,
				Status = "Active",
				Credits = course.Credits
			};

			await _repo.AddEnrollmentAsync(enrollment);
			return enrollmentId;
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
