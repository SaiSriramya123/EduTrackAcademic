using EduTrackAcademics.Data;
using EduTrackAcademics.Model;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Repository
{
	public class InstructorRepo : IInstructorRepo
	{
		private readonly EduTrackAcademicsContext _context;

		public InstructorRepo(EduTrackAcademicsContext context)
		{
			_context = context;
		}

		public async Task<object> GetBatches(string instructorId)
		{
			return await _context.CourseBatches
				.Where(b => b.InstructorId == instructorId)
				.Select(b => new
				{
					b.BatchId,
					b.MaxStudents,
					b.CurrentStudents,
					b.IsActive,
					b.Course.CourseId,
					b.Course.CourseName
				}).ToListAsync();
		}

		public async Task<object> GetStudents(string batchId)
		{
			return await _context.StudentBatchAssignments
				.Where(s => s.BatchId == batchId)
				.Select(s => new
				{
					s.Student.StudentId,
					s.Student.StudentName,
					s.Student.StudentEmail,
					s.Student.StudentPhone
				}).ToListAsync();
		}

		public async Task<object> GetDashboard(string instructorId)
		{
			var dashboard = await (
				from batch in _context.CourseBatches
				where batch.InstructorId == instructorId

				join course in _context.Course
					on batch.CourseId equals course.CourseId

				join sba in _context.StudentBatchAssignments
					on batch.BatchId equals sba.BatchId into sbagroup

				select new
				{
					batch.BatchId,
					batch.CourseId,
					CourseName = course.CourseName,

					Students = sbagroup.Select(s => new
					{
						s.Student.StudentId,
						s.Student.StudentName,
						s.Student.StudentEmail,
						s.Student.StudentPhone
					}).ToList()
				}
			).ToListAsync();

			return dashboard;
		}

		public async Task AddModuleAsync(Module module)
		{
			await _context.Modules.AddAsync(module);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateModuleAsync(string id, Module module)
		{
			var existing = await _context.Modules.FindAsync(id);

			if (existing == null)
				throw new KeyNotFoundException("Module not found");

			existing.Name = module.Name;
			existing.SequenceOrder = module.SequenceOrder;
			existing.LearningObjectives = module.LearningObjectives;

			await _context.SaveChangesAsync();
		}

		public async Task DeleteModuleAsync(string id)
		{
			var module = await _context.Modules.FindAsync(id);
			if (module == null)
				throw new KeyNotFoundException("Module not found");
			_context.Modules.Remove(module);
			await _context.SaveChangesAsync();
		}

		public async Task<object> GetModules(string courseId)
		{
			return await _context.Modules.Where(m => m.CourseID == courseId).ToListAsync();
		}

		public async Task<bool> CompleteModule(string moduleId)
		{
			return await _context.Contents.AnyAsync(c => c.ModuleID == moduleId);
		}

		public async Task AddContent(Content c)
		{
			_context.Contents.Add(c);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateContent(Content c)
		{
			_context.Contents.Update(c);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteContent(string id)
		{
			var content = await _context.Contents.FindAsync(id);
			if (content == null)
				return;
			_context.Contents.Remove(content);
			await _context.SaveChangesAsync();
		}

		public async Task<object> GetContent(string moduleId)
		{
			return await _context.Contents.Where(c => c.ModuleID == moduleId).ToListAsync();
		}

		public async Task AddAssessment(Assessment a)
		{
			_context.Assessments.Add(a);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAssessment(Assessment a)
		{
			_context.Assessments.Update(a);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAssessment(string id)
		{
			var assessment = await _context.Assessments.FindAsync(id);
			if (assessment == null)
				return;
			_context.Assessments.Remove(assessment);
			await _context.SaveChangesAsync();
		}

		public async Task<object> GetAssessments(string courseId)
		{
			return await _context.Assessments.Where(a => a.CourseID == courseId).ToListAsync();
		}

		public async Task<object> GetQuestions(string assessmentId)
		{
			return await _context.Questions.Where(q => q.AssessmentId == assessmentId).ToListAsync();
		}

		public async Task EvaluateAssessment(string id, int marks, string feedback)
		{
			var a = await _context.Assessments.FindAsync(id);
			a.MarksObtained = marks;
			a.Feedback = feedback;
			a.Status = "Evaluated";
			await _context.SaveChangesAsync();
		}

		public async Task MarkAttendance(Attendance a)
		{
			a.SessionDate = DateTime.Now;
			_context.Attendances.Add(a);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAttendance(string id, Attendance u)
		{
			var r = await _context.Attendances.FindAsync(id);
			r.Status = u.Status;
			r.Mode = u.Mode;
			r.UpdateReason = u.UpdateReason;
			r.UpdatedOn = DateTime.Now;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAttendance(string id, string reason)
		{
			var record = await _context.Attendances.FindAsync(id);
			if (record == null)
				return;
			record.IsDeleted = true;
			record.DeletionReason = reason;
			record.DeletionDate = DateTime.Now;
			await _context.SaveChangesAsync();
		}

		public async Task<object> GetAttendance(string batchId)
		{
			return await _context.Attendances
				.Where(a => a.BatchId == batchId && !a.IsDeleted)
				.ToListAsync();
		}

		public async Task<object> GetAttendanceReport(string batchId)
		{
			return await _context.Attendances
				.Where(a => a.BatchId == batchId && !a.IsDeleted)
				.GroupBy(a => a.StudentBatchAssignment.Student.StudentName)
				.Select(g => new
				{
					Student = g.Key,
					Present = g.Count(x => x.Status),
					Absent = g.Count(x => !x.Status)
				}).ToListAsync();
		}

		public async Task<object> GetIrregularStudents(string batchId)
		{
			return await _context.Attendances
				.Where(a => a.BatchId == batchId && !a.IsDeleted)
				.GroupBy(a => a.StudentBatchAssignment.Student.StudentName)
				.Select(g => new
				{
					Student = g.Key,
					Absences = g.Count(x => !x.Status)
				})
				.Where(x => x.Absences > 3)
				.ToListAsync();
		}
	}
}
