using EduTrackAcademics.Model;

namespace EduTrackAcademics.Services
{
    public interface IAcademicReportService
    {
        List<AcademicReport> GetBatches();
        AcademicReport GetBatchDetails(string reportId);
    }
}
