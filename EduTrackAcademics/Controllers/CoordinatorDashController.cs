using EduTrackAcademics.Data;
using Microsoft.AspNetCore.Mvc;
using EduTrackAcademics.Services;

namespace EduTrackAcademics.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CoordinatorDashController : ControllerBase
	{
		//private readonly EduTrackAcademicsContext _context;
		private readonly ICoordinatorService _coordinatorService;

		public CoordinatorDashController(ICoordinatorService coordinatorService)
		{
			//_context = context;
			_coordinatorService = coordinatorService;
		}

		[HttpGet]
		public ActionResult<List<string>> GetInstructorDetails()
		{
			var result = _coordinatorService.GetInstructorDetails();
			return Ok(result);
		}
		
	}
}
