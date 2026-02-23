using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackAcademics.Controllers
{
	[ApiController]
	[Route("api/admin")]
	public class AdminDashboardController : ControllerBase
	{
		private readonly EduTrackAcademicsContext _context;
		private readonly IdService _idService;

		public AdminDashboardController(EduTrackAcademicsContext context, IdService idService)
		{
			_context = context;
			_idService = idService;
		}

		[HttpPost("qualification")]
		public IActionResult AddQualification([FromBody] QualificationDTO dto)
		{
			var qualification = new Qualification
			{
				QualificationId = _idService.GenerateQualificationId(),
				QualificationName = dto.QualificationName
			};

			_context.Qualification.Add(qualification);
			_context.SaveChanges();
			return Ok(new { Message = "Qualification added", id = qualification.QualificationId });
		}

		[HttpPost("program")]
		public IActionResult AddProgram([FromBody] ProgramDTO dto)
		{
			// Check if qualification exists
			var qualification = _context.Qualification.FirstOrDefault(q => q.QualificationId == dto.QualificationId);
			if (qualification == null) return BadRequest("Qualification does not exist.");

			var program = new ProgramEntity
			{
				ProgramId = _idService.GenerateProgramId(),
				ProgramName = dto.ProgramName,
				QualificationId = dto.QualificationId
			};

			_context.Programs.Add(program);
			_context.SaveChanges();
			return Ok(new { Message = "Program added", id = program.ProgramId });
		}

		[HttpPost("academic-year")]
		public IActionResult AddAcademicYear([FromBody] AcademicYearDTO dto)
		{
			// Check if program exists
			var program = _context.Programs.FirstOrDefault(p => p.ProgramId == dto.ProgramId);
			if (program == null) return BadRequest("Program does not exist.");

			var year = new AcademicYear
			{
				AcademicYearId = _idService.GenerateAcademicYearId(),
				YearNumber = dto.YearNumber,
				ProgramId = dto.ProgramId
			};

			_context.AcademicYear.Add(year);
			_context.SaveChanges();
			return Ok(new { Message = "Academic year added", id = year.AcademicYearId });
		}
	}

}
