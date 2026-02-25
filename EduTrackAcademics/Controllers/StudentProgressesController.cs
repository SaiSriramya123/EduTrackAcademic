using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exception;
using EduTrackAcademics.Model;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentProgressesController : ControllerBase
    {
		private readonly IStudentProgressesService _service;

		public StudentProgressesController(IStudentProgressesService service)
		{
			_service = service;
		}

		[HttpPost("complete-content")]
		public async Task<IActionResult> AddProgressRecord([FromBody] StudentProgressDto dto)
		{
			try
			{
				var result = await _service.AddProgressRecordAsync(dto);

				return Ok(new
				{
					status = 200,
					data = result
				});
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500, new
				{
					status = 500,
					message = "Internal Server Error",
					error = ex.Message
				});
			}
			//var result = await _service.AddProgressRecordAsync(dto);
			//return Ok(new { status = 200, data = result });
		}
	}
}
