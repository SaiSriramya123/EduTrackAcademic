using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrackAcademics.Model
{
	public class Coordinator
	{
		[Key]
		public string CoordinatorId { get; set; }
		[Required]
		public string CoordinatorName { get; set; }

		[Required, EmailAddress]
		public string CoordinatorEmail { get; set; }

		public string Role { get; set; } 

		public long CoordinatorPhone { get; set; }

		public string CoordinatorQualification { get; set; }

		public string CoordinatorExperience { get; set; }

		public string CoordinatorGender { get; set; }
		[Required]
		[NotMapped]
		public IFormFile Resume { get; set; }

		[Required]
		public string ResumePath{ get; set; }

		[Required]
		public string CoordinatorPassword { get; set; }
		[Required]
		public bool IsFirstLogin { get; set; }
		[Required]
		public bool IsActive { get; set; }


	}
}
