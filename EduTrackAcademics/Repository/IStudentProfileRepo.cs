using EduTrackAcademics.DTO;

namespace EduTrackAcademics.Repository
{
	public interface IStudentProfileRepo
	{
		
		public interface IStudentProfileRepo
		{
			Task<StudentDTO?> GetProgramDetailsAsync(string studentId);
			Task<StudentDTO?> GetPersonalInfoAsync(string studentId);
			Task<bool> StudentExistsAsync(string studentId);
			Task UpdateAdditionalInfoAsync(string studentId, StudentAdditionalDetailsDTO dto);
		}
	}
}

