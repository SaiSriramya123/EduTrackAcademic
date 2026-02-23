using System.ComponentModel.DataAnnotations;

namespace EduTrackAcademics.Model
{
	public class StudentBatchAssignment
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string BatchId { get; set; }

		[Required]
		public string StudentId { get; set; }
	}
}