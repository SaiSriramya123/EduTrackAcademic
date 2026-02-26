
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrackAcademics.Model
{
	public class Course
	{
		[Key]
		public string CourseId { get; set; }

		[Required]
		public string CourseName { get; set; }

		public int Credits { get; set; }
		public int DurationInWeeks { get; set; }

		[Required]
		public string AcademicYearId { get; set; }

		[ForeignKey(nameof(AcademicYearId))]
		public AcademicYear AcademicYear { get; set; }
<<<<<<< HEAD
        public List<Assessment> Assessments { get; set; } = new();
    }
=======
		public ICollection<Enrollment> Enrollments { get; set; }
	}
>>>>>>> 60f6f5c889e1b077c3e5d0b312ad007a2dbce71e
}
