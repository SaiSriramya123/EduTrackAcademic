using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrackAcademics.Model
{
	public class Assessment
	{
		[Key]
		[Required]
		[RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "AssessmentID must be alphanumeric.")]
		public string AssessmentID { get; set; }

		[Required]
		[ForeignKey("Course")]
		public string CourseID { get; set; }
		public Course Course { get; set; }

		[Required]
		[RegularExpression(@"^(Assignment|Quiz|Exam)$", ErrorMessage = "Type must be Assignment, Quiz, or Exam.")]
		public string Type { get; set; }

		[Required]
		[Range(1, 100, ErrorMessage = "Marks must be between 1 and 100")]
		public int MaxMarks { get; set; }
		[Required]
		public DateTime DueDate { get; set; }
		[Required]
		[RegularExpression(@"^(Open|Closed)$", ErrorMessage = "Status must be Open or Closed.")]
		public string Status { get; set; }

		public int? MarksObtained { get; set; }
		public string? Feedback { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public List<Question> Questions { get; set; } = new();
	}
}
