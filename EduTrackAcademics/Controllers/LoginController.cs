using System.Data;
using EduTrackAcademics.AuthFolder;
using EduTrackAcademics.AuthFolder;
using EduTrackAcademics.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;


namespace EduTrackAcademics.Controllers
{
		[ApiController]
		[Route("api/auth")]
		public class LoginAuthorizationController : ControllerBase
		{
			private readonly IAuthorization _authService;

			public LoginAuthorizationController(IAuthorization authService)
			{
				_authService = authService;
			}


		[Authorize(Roles = "Student")]
		[HttpPost("student-login")]
			public IActionResult StudentLogin([FromBody] Student student)
			{
				var token = _authService.AuthenticateStudent(student);
				if (token == null) return Unauthorized(new { Message = "Invalid student credentials." });

				return Ok(new { Message = "Login successful", Role = "Student", Token = token });
			}

	      	[Authorize(Roles = "Instructor")]
			[HttpPost("instructor-login")]
			public IActionResult InstructorLogin([FromBody] Instructor instructor)
			{
				var token = _authService.AuthenticateInstructor(instructor);
				if (token == null) return Unauthorized(new { Message = "Invalid instructor credentials." });

				return Ok(new { Message = "Login successful", Role = "Instructor", Token = token });
			}

		[Authorize(Roles = "Coordinator")]
		[HttpPost("coordinator-login")]
			public IActionResult CoordinatorLogin([FromBody] Coordinator coordinator)
			{
				var token = _authService.AuthenticateCoordinator(coordinator);
				if (token == null) 
				return Unauthorized(new { Message = "Invalid coordinator credentials."
				});

				return Ok(new { Message = "Login successful", Role = "Coordinator", Token = token });
			}

			[HttpPost("logout/{userId}")]
			public IActionResult Logout(string userId)
			{
				var result = _authService.Logout(userId);

				if (!result)
					return BadRequest(new { Message = "Logout failed." });

				return Ok(new { Message = "Logout successful." });
			}
		}
}

