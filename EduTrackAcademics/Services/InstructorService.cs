using EduTrackAcademics.Data;
using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Services
{
	public class InstructorService : IInstructorService
	{
		private readonly IInstructorRepo _repo;

		public InstructorService(IInstructorRepo repo)
		{
			_repo = repo;
		}

		public Task<object> GetBatches(string instructorId) 
			=> _repo.GetBatches(instructorId);
		public Task<object> GetStudents(string batchId) 
			=> _repo.GetStudents(batchId);
		public Task<object> GetDashboard(string instructorId) 
			=> _repo.GetDashboard(instructorId);

		public Task AddModule(Module module) 
			=> _repo.AddModule(module);
		public Task UpdateModule(Module module) 
			=> _repo.UpdateModule(module);
		public Task DeleteModule(string id) 
			=> _repo.DeleteModule(id);
		public Task<object> GetModules(string courseId) 
			=> _repo.GetModules(courseId);
		public Task<bool> CompleteModule(string moduleId) 
			=> _repo.CompleteModule(moduleId);

		public Task AddContent(Content content) 
			=> _repo.AddContent(content);
		public Task UpdateContent(Content content) 
			=> _repo.UpdateContent(content);
		public Task DeleteContent(string id) 
			=> _repo.DeleteContent(id);
		public Task<object> GetContent(string moduleId) 
			=> _repo.GetContent(moduleId);

		public Task AddAssessment(Assessment a) 
			=> _repo.AddAssessment(a);
		public Task UpdateAssessment(Assessment a) 
			=> _repo.UpdateAssessment(a);
		public Task DeleteAssessment(string id) 
			=> _repo.DeleteAssessment(id);
		public Task<object> GetAssessments(string courseId) 
			=> _repo.GetAssessments(courseId);
		public Task<object> GetQuestions(string assessmentId) 
			=> _repo.GetQuestions(assessmentId);
		public Task EvaluateAssessment(string id, int marks, string feedback) 
			=> _repo.EvaluateAssessment(id, marks, feedback);

		public Task MarkAttendance(Attendance a) 
			=> _repo.MarkAttendance(a);
		public Task UpdateAttendance(string id, Attendance updated) 
			=> _repo.UpdateAttendance(id, updated);
		public Task DeleteAttendance(string id, string reason) 
			=> _repo.DeleteAttendance(id, reason);
		public Task<object> GetAttendance(string batchId) 
			=> _repo.GetAttendance(batchId);
		public Task<object> GetAttendanceReport(string batchId) 
			=> _repo.GetAttendanceReport(batchId);
		public Task<object> GetIrregularStudents(string batchId) 
			=> _repo.GetIrregularStudents(batchId);
	}
}
