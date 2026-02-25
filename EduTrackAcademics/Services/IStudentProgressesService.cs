using System.Threading.Tasks;
using EduTrackAcademics.DTO;
namespace EduTrackAcademics.Services
{
	public interface IStudentProgressesService
	{
		Task<int> AddProgressRecordAsync(StudentProgressDto dto);
	}
}
