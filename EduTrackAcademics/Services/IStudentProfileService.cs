using System.Threading.Tasks;
using EduTrackAcademics.DTO;
namespace EduTrackAcademics.Services
{
	public interface IStudentProfileService
	{
		Task<StudentDTO> GetPersonalInfoAsync(string studentId);
		Task<StudentDTO> GetProgramDetails(string studentId);
		Task UpdateAdditionalInfo(string studentId, StudentAdditionalDetailsDTO dto);
	}
}