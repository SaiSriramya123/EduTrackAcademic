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

		public List<CourseBatch> GetBatches(string instructorId)
		{
			return _context.CourseBatches
				.Where(b => b.InstructorId == instructorId)
				.Include(b => b.Course)
				.ToList();
		}

		public List<StudentBatchAssignment> GetStudents(string batchId)
		{
			return _context.StudentBatchAssignments
				.Where(s => s.BatchId == batchId)
				.Include(s => s.Student)
				.ToList();
		}

		public List<Module> GetModules(string courseId)
		{
			return _context.Modules
				.Where(m => m.CourseID == courseId)
				.ToList();
		}

		public List<Content> GetContent(string moduleId)
		{
			return _context.Contents
				.Where(c => c.ModuleID == moduleId)
				.ToList();
		}

		public List<Assessment> GetAssessments(string courseId)
		{
			return _context.Assessments
				.Where(a => a.CourseID == courseId)
				.Include(a => a.Questions)
				.ToList();
		}

		public List<Question> GetQuestions(string assessmentId)
		{
			return _context.Questions
				.Where(q => q.AssessmentId == assessmentId)
				.ToList();
		}

		public void Evaluate(string assessmentId, int marks, string feedback)
		{
			var assessment = _context.Assessments.Find(assessmentId);
			if (assessment == null) return;

			assessment.MarksObtained = marks;
			assessment.Feedback = feedback;

			_context.SaveChanges();
		}

		public void SaveAttendance(Attendance attendance)
		{
			_context.Attendances.Add(attendance);
			_context.SaveChanges();
		}

		public List<Attendance> GetAttendance(string batchId)
		{
			return _context.Attendances
				.Where(a => a.BatchID == batchId)
				.ToList();
		}
	}
}
