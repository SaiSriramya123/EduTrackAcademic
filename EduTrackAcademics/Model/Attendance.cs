using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrackAcademics.Model
{
	public class Attendance
	{
		[Key]
		[Required]
		[RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "AttendanceID must be alphanumeric.")]
		public string AttendanceID { get; set; }

		[Required]
		//[ForeignKey("Enrollment")]
		public string EnrollmentID { get; set; }
		//public Enrollment Enrollment { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime SessionDate { get; set; }
		[Required]
		[RegularExpression(@"^(Online|Classroom)$", ErrorMessage = "Mode must be Online or Classroom.")]
		public string Mode { get; set; }
		[Required]
		public bool Status { get; set; }
	}
}
