using System.ComponentModel.DataAnnotations;

namespace EduTrackAcademics.Model
{
	public class StudentBatchAssignment
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string BatchId { get; set; }
		public CourseBatch Batch { get; set; }	

		[Required]
		public string StudentId { get; set; }
		public Student Student { get; set; } 
	}
}