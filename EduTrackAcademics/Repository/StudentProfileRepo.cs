using Microsoft.EntityFrameworkCore;
using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
namespace EduTrackAcademics.Repository
{
	public class StudentProfileRepo : IStudentProfileRepo
	{
		private readonly EduTrackAcademicsContext _context;
		public StudentProfileRepo(EduTrackAcademicsContext context)
		{
			_context = context;
		}
		public async Task<bool> StudentExistsAsync(string studentId)
		{
			return await _context.Student
				.AsNoTracking()
				.AnyAsync(s => s.StudentId == studentId);
		}
		public async Task<StudentDTO> GetPersonalInfoAsync(string studentId)
		{
			return await _context.Student
				.Where(s => s.StudentId == studentId)
				.Select(s => new StudentDTO
				{
					StudentName = s.StudentName,
					StudentEmail = s.StudentEmail,
					StudentPhone = s.StudentPhone,
					StudentGender = s.StudentGender
				})
				.AsNoTracking()
				.FirstOrDefaultAsync();
		}
		public async Task<StudentDTO?> GetProgramDetailsAsync(string studentId)
		{
			return await _context.Student
				.Where(s => s.StudentId == studentId)
				.Select(s => new StudentDTO
				{
					StudentQualification = s.StudentQualification,
					StudentProgram = s.StudentProgram,
					StudentAcademicYear = s.StudentAcademicYear
				})
				.AsNoTracking()
				.FirstOrDefaultAsync();
		}
		public async Task UpdateAdditionalInfoAsync(string studentId, StudentAdditionalDetailsDTO dto)
		{
			var existing = await _context.StudentAdditionalDetails
				.FirstOrDefaultAsync(a => a.StudentId == studentId);
			if (existing == null)
			{
				var newDetail = new StudentAdditionalDetails
				{
					StudentId = studentId,
					Nationality = dto.Nationality,
					Citizenship = dto.Citizenship,
					DayscholarHosteller = dto.dayscholarHosteller,
					GateScore = dto.GateScore,
					Certifications = dto.Certifications,
					Clubs_Chapters = dto.Clubs_Chapters,
					Achievements = dto.Achievements,
					EducationGap = dto.EducationGap
				};
				await _context.StudentAdditionalDetails.AddAsync(newDetail);
			}
			else
			{
				existing.Nationality = dto.Nationality;
				existing.Citizenship = dto.Citizenship;
				existing.DayscholarHosteller = dto.dayscholarHosteller;
				existing.GateScore = dto.GateScore;
				existing.Certifications = dto.Certifications;
				existing.Clubs_Chapters = dto.Clubs_Chapters;
				existing.Achievements = dto.Achievements;
				existing.EducationGap = dto.EducationGap;
			}
			await _context.SaveChangesAsync();
		}
	}
}