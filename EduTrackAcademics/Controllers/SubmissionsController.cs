using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTrackAcademics.Data;
using EduTrackAcademics.Model;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduTrackAcademics.DTO;

namespace EduTrackAcademics.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SubmissionController : ControllerBase
	{
		private readonly ISubmissionService _service;

		public SubmissionController(ISubmissionService service)
		{
			_service = service;
		}

		[HttpGet("view-assessments")]
		public async Task<IActionResult> ViewAssessments(FetchAssessmentDto dto)
		{
			try
			{
				var data = await _service.GetStudentAssessmentsAsync(dto.StudentId);

				if (data == null || !data.Any())
				{
					return NotFound(new
					{
						status = 404,
						message = "No assessments found"
					});
				}

				return Ok(new
				{
					status = 200,
					data = data
				});
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500, new
				{
					status = 500,
					message = ex.Message
				});
			}
		}

		[HttpGet("start-assessment")]
		public async Task<IActionResult> StartAssessment(string studentId, string assessmentId)
		{
			try
			{
				var questions = await
					_service.StartAssessmentAsync(
						studentId, assessmentId);

				return Ok(new
				{
					status = 200,
					data = questions
				});
			}
			catch (ApplicationException ex)
			{
				return BadRequest(new
				{
					status = 400,
					message = ex.Message
				});
			}
		}
	}
}
