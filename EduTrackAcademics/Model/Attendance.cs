using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrackAcademics.Model
{
	public class Attendance
	{
		[Key]
		[Required]
		[RegularExpression(@"AT[0-9]{3, }$", ErrorMessage = "AttendanceID must be like AT001")]
		public string AttendanceID { get; set; }

		[Required(ErrorMessage = "Enrollment ID is required")]
		[ForeignKey("Enrollment")]
		public string EnrollmentID { get; set; }
		//public Enrollment Enrollment { get; set; }

		[Required(ErrorMessage = "Batch ID is required")]
		[ForeignKey("Batch")]
		public string BatchID { get; set; }
		public Batch Batch { get; set; }

		[Required(ErrorMessage = "Session Date is required")]
		[DataType(DataType.Date)]
		public DateTime SessionDate { get; set; }


		[Required]
		[RegularExpression(@"^(Online|Classroom)$", ErrorMessage = "Mode must be Online or Classroom.")]
		public string Mode { get; set; }
		[Required]
		public bool Status { get; set; }

		public string? UpdateReason { get; set; }
		public DateTime? UpdatedOn { get; set; }

		public bool IsDeleted { get; set; } = false;
		public string? DeletionReason { get; set; }
		public DateTime? DeletionDate { get; set; }
	}
	}
