using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

 
public class CoordinatorDTO
{
	[Required]
	public string CoordinatorName { get; set; }

	[Required, EmailAddress]
	public string CoordinatorEmail { get; set; }

	public long CoordinatorPhone { get; set; }

	public string CoordinatorQualification { get; set; }

	public string CoordinatorExperience { get; set; }

	public string CoordinatorGender { get; set; }

	[Required]
	public IFormFile Resume { get; set; }
}