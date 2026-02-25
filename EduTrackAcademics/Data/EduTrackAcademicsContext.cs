using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Data
{
    public class EduTrackAcademicsContext : DbContext
    {
        public EduTrackAcademicsContext (DbContextOptions<EduTrackAcademicsContext> options)
            : base(options)
        {
        }

        public DbSet<EduTrackAcademics.Model.Course> Course { get; set; } = default!;
        public DbSet<EduTrackAcademics.Model.Student> Student { get; set; }
		public DbSet<EduTrackAcademics.Model.StudentAdditionalDetails> StudentAdditionalDetails { get; set; }
		public DbSet<EduTrackAcademics.Model.StudentLoginHistory> AuditLog { get; set; }
		public DbSet<EduTrackAcademics.Model.Instructor> Instructor { get; set; }
		public DbSet<EduTrackAcademics.Model.Coordinator> Coordinator{ get; set; }
		public DbSet<EduTrackAcademics.Model.Qualification>Qualification { get; set; }
		public DbSet<EduTrackAcademics.Model.AcademicYear> AcademicYear { get; set; }
		public DbSet<EduTrackAcademics.Model.CourseAssignment>CourseAssignment{ get; set; }
		public DbSet<EduTrackAcademics.Model.ProgramEntity> Programs { get; set; }
		public DbSet<EduTrackAcademics.Model.Question> Questions { get; set; }
		public DbSet<EduTrackAcademics.Model.Assessment> Assessments { get; set; }
		public DbSet<EduTrackAcademics.Model.Attendance> Attendances { get; set; }
		public DbSet<EduTrackAcademics.Model.Module>Modules { get; set; }
		public DbSet<EduTrackAcademics.Model.Content> Contents { get; set; }
		public DbSet<StudentCourseAssignment> StudentCourseAssignments { get; set; }
		public DbSet<InstructorCourseAssignment> InstructorCourseAssignments { get; set; }
		public DbSet<CourseBatch> CourseBatches { get; set; }
		public DbSet<Notification> Notification{ get; set; }
		public DbSet<StudentBatchAssignment> StudentBatchAssignments { get; set; }
		
		public DbSet<StudentLoginHistory>StudentLoginHistories { get; set; }
		public DbSet<Performance>Performances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Qualification>()
				.HasMany(q => q.Programs)
				.WithOne(p => p.Qualification)
				.HasForeignKey(p => p.QualificationId);

			modelBuilder.Entity<ProgramEntity>()
				.HasMany(p => p.AcademicYears)
				.WithOne(y => y.Program)
				.HasForeignKey(y => y.ProgramId);

			modelBuilder.Entity<AcademicYear>()
				.HasMany(y => y.Courses)
				.WithOne(c => c.AcademicYear)
				.HasForeignKey(c => c.AcademicYearId);
		}
        public DbSet<EduTrackAcademics.Model.Enrollment> Enrollment { get; set; } = default!;

		public DbSet<EduTrackAcademics.Model.StudentProgress> StudentProgress { get; set; } = default!;
	}
}
