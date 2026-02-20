using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrackAcademics.Model
{
	public class Module
	{
		[Key]
		[Required]
		[RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "ModuleIDmust be AlphaNumeric.")]
		public string ModuleID { get; set; }

		[Required]
		[ForeignKey("Course")]
		public string CourseID { get; set; }
		public Course Course { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 3)]
		[RegularExpression(@"^[A-Za-z0-9\s\-\.,]+$", ErrorMessage = "Module Name contains invalid characters.")]
		public string Name { get; set; }
		[Required]
		[Range(1, 100)]
		public int SequenceOrder { get; set; }
		[Required]
		[StringLength(500)]
		public string LearningObjectives { get; set; }
	}
}
