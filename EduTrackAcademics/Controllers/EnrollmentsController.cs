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
		public class EnrollmentController : ControllerBase
		{
			private readonly IEnrollmentService _service;

			public EnrollmentController(IEnrollmentService service)
			{
				_service = service;
			}

			[HttpPost]
			public async Task<IActionResult> AddEnrollment([FromBody] EnrollmentDto dto)
			{
				try
				{
					var result = await _service.AddEnrollmentAsync(dto);

					return Ok(new
					{
						status = 200,
						data = result
					});
				}
				catch (ApplicationException ex)
				{
					return StatusCode(500,
					new { status = 500, message = ex.Message });
				}
			//var result = await _service.AddEnrollmentAsync(dto);
				//return Ok(new { status = 200, data = result });
			}

			[HttpGet("content")]
			public async Task<IActionResult> ViewCourseContent(EnrollmentDto dto)
			{
			try
			{
				var modules = await _service
					.GetContentForStudentAsync(dto.StudentId, dto.CourseId);

				return Ok(new { status = 200, data = modules });
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500,
					new { status = 500, message = ex.Message });
			}
			//var modules = await _service.GetContentForStudentAsync(dto.StudentId, dto.CourseId);
			//return Ok(new { status = 200, data = modules });
		}

			[HttpGet("progress")]
			public async Task<IActionResult> GetCourseProgress(EnrollmentDto dto)
			{
			try
			{
				var progress = await _service
					.GetCourseProgressPercentageAsync(dto.StudentId, dto.CourseId);

				return Ok(new { status = 200, progress });
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500,
					new { status = 500, message = ex.Message });
			}
			//var progress = await _service.GetCourseProgressPercentageAsync(dto.StudentId, dto.CourseId);
			//return Ok(new { status = 200, progress });
		}

			[HttpPost("update-status")]
			public async Task<IActionResult> ProcessCourseCompletion(EnrollmentDto dto)
			{
			try
			{
				var result = await _service
					.ProcessCourseCompletionAsync(dto.StudentId, dto.CourseId);

				return Ok(new
				{
					status = 200,
					message = result
						? "Completed Successfully"
						: "Progress not 100%"
				});
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500,
					new { status = 500, message = ex.Message });
			}
			//var result = await _service.ProcessCourseCompletionAsync(dto.StudentId, dto.CourseId);

			//return Ok(new
			//{
			//	status = 200,
			//	message = result ? "Completed Successfully" : "Progress not 100%"
			//});
		}
		}
}

