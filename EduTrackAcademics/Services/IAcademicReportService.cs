using EduTrackAcademics.Model;

namespace EduTrackAcademics.Services
{
    public interface IAcademicReportService
    {
        AcademicReport GetSingleReport(String batchId);
        List<AcademicReport> GetReport();
    }
}
