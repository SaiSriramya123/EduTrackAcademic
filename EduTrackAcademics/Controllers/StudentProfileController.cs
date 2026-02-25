using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;
using EduTrackAcademics.DTO;
using System;
using System.Threading.Tasks;

namespace EduTrackAcademics.Controllers
{
	[ApiController]
	[Route("api/profile")]
	public class StudentProfileController : ControllerBase
	{
		private readonly IStudentProfileService _service;
		public StudentProfileController(IStudentProfileService service)
		{
			_service = service;
		}

		[HttpGet("personal-info/{studentId}")]
		public async Task<IActionResult> GetPersonalInfo(string studentId)
		{
			try
			{
				var result = await _service.GetPersonalInfoAsync(studentId);
				return Ok(result);
			}
			catch (ArgumentException ex)
			{
				return NotFound(new { Message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
			}
		}

		[HttpGet("program-details/{studentId}")]
		public async Task<IActionResult> GetProgramDetails(string studentId)
		{
			try
			{
				var result = await _service.GetProgramDetails(studentId);
				return Ok(result);
			}
			catch (ArgumentException ex)
			{
				return NotFound(new { Message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
			}
		}

		[HttpPut("additional-info/{studentId}")]
		public async Task<IActionResult> UpdateAdditionalInfo(string studentId, [FromBody] StudentAdditionalDetailsDTO dto)
		{
			try
			{
				await _service.UpdateAdditionalInfo(studentId, dto);
				return Ok(new { Message = "Additional information updated successfully." });
			}
			catch (ArgumentException ex)
			{
				return NotFound(new { Message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
			catch (System.Exception ex)
			{
				return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
			}
		}
	}
}
