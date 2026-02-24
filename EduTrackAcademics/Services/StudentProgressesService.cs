using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exception;
using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;

namespace EduTrackAcademics.Services
{
	public class StudentProgressesService: IStudentProgressesService
	{
		private readonly IStudentProgressesRepository _repo;

		public StudentProgressesService(IStudentProgressesRepository repo)
		{
			_repo = repo;
		}
		public async Task<int> AddProgressRecordAsync(StudentProgressDto dto)
		{
			if (await _repo.CheckIfProgressExistsAsync(dto.StudentId, dto.ContentId))
				throw new ProgressRecordAlreadyExistsException("Already completed");

			int count = await _repo.GetProgressCountAsync();
			string newId = $"sp_{(count + 1):D3}";

			var progress = new StudentProgress
			{
				ProgressID = newId,
				StudentId = dto.StudentId,
				ContentId = dto.ContentId,
				IsCompleted = true,
				CompletionDate = DateTime.Now
			};

			return await _repo.AddProgressRecordAsync(progress);

			//return new ProgressResponseDto
			//{
			//	ProgressId = newId,
			//	IsCompleted = true,
			//	CompletionDate = progress.CompletionDate
			//};
		}
	}
}
