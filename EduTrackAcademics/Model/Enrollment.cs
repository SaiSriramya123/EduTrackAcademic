using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrackAcademics.Model
{
	public class Enrollment
	{
		[Key]
		[Required]
		public string EnrollmentId { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime EnrollmentDate { get; set; }

		[Required]
		[StringLength(20)]
		[RegularExpression(@"^(Active|Completed|Dropped)$", ErrorMessage = "Status must be Active, Completed, or Dropped.")]
		public string Status { get; set; }

		[Required]
		public int credits { get; set; } = 0;

		[ForeignKey("Student")]
		public string StudentId { get; set; }

		[ForeignKey("Course")]
		public string CourseId { get; set; }
	}
}
