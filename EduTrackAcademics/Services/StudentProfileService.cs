using EduTrackAcademics.Exception;
using EduTrackAcademics.Repository;
using Microsoft.EntityFrameworkCore;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exception;
namespace EduTrackAcademics.Services
{
	public class StudentProfileService : IStudentProfileService
	{
		private readonly IStudentProfileRepo _repo;
		public StudentProfileService(IStudentProfileRepo repo)
		{
			_repo = repo;
		}
		public async Task<StudentDTO> GetPersonalInfoAsync(string studentId)
		{
			var result = await _repo.GetPersonalInfoAsync(studentId);
			if (result == null)
				throw new StudentNotFoundException("Student not found");
			return result;
		}
		public async Task<StudentDTO> GetProgramDetails(string studentId)
		{
			var result = await _repo.GetProgramDetailsAsync(studentId);
			if (result == null)
				throw new StudentNotFoundException("Student not found");
			return result;
		}
		public async Task UpdateAdditionalInfo(string studentId, StudentAdditionalDetailsDTO dto)
		{
			var exists = await _repo.StudentExistsAsync(studentId);
			if (!exists)
				throw new StudentNotFoundException("Student not found");
			await _repo.UpdateAdditionalInfoAsync(studentId, dto);
		}

	}
}
