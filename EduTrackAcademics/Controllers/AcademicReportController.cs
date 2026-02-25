using EduTrackAcademics.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AcademicReportController : ControllerBase
{
    private readonly EduTrackAcademicsContext _context;

    public AcademicReportController(EduTrackAcademicsContext context)
    {
        _context = context;
    }

    // ===============================
    // GET ALL BATCHES WITH STUDENTS
    // ===============================
    [HttpGet("batches")]
    public IActionResult GetBatchesWithStudents()
    {
        var data = _context.CourseBatches
            .Select(b => new
            {
                b.BatchId,
                b.CourseId,
                b.MaxStudents,
                b.CurrentStudents,
            

                Students = _context.StudentBatchAssignments
                    .Where(sba => sba.BatchId == b.BatchId)
                    .Select(sba => new
                    {
                        sba.Student.StudentId,
                        sba.Student.StudentName,
                        sba.Student.StudentEmail
                    })
                    .ToList()
            })
            .ToList();

        return Ok(data);
    }
}
