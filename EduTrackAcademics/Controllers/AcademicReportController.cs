using EduTrackAcademics.Data;
using EduTrackAcademics.Exception;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;
[ApiController]

[Route("api/[controller]")]

public class AcademicReportController : ControllerBase

{

    private readonly IAcademicReportService _service;

    public AcademicReportController(IAcademicReportService service)

    {

        _service = service;

    }

    [HttpGet("get-single-report/{batchId}")]

    public IActionResult GetSingleReport(string batchId)

    {

        try

        {

            var result = _service.GetSingleReport(batchId);

            return Ok(result);

        }

        catch (BatchNotFoundException ex)

        {

            return NotFound(ex.Message);

        }

        catch (NoStudentsFoundException ex)

        {

            return NotFound(ex.Message);

        }

    }

}
