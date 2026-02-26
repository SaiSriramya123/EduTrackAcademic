using System;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Services
{
	public interface IEnrollmentService
	{
		Task<string> AddEnrollmentAsync(EnrollmentDto dto);
		Task<List<Module>> GetContentForStudentAsync(string studentId, string courseId);
		Task<double> GetCourseProgressPercentageAsync(string studentId, string courseId);
		Task<bool> ProcessCourseCompletionAsync(string studentId, string courseId);
	}
}
