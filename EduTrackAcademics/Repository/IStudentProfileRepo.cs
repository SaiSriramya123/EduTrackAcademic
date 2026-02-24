using EduTrackAcademics.DTO;

namespace EduTrackAcademics.Repository
{
	public interface IStudentProfileRepo
	{
		
		
			Task<StudentDTO?> GetPersonalInfoAsync(string studentId);
			Task<StudentDTO?> GetProgramDetailsAsync(string studentId);
			Task<bool> StudentExistsAsync(string studentId);
			Task UpdateAdditionalInfoAsync(string studentId, StudentAdditionalDetailsDTO dto);
		
	}
}

