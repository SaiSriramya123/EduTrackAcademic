using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
    public interface IAcademicReportRepository
    {
        
        List<AcademicReport> GetBatches();
        AcademicReport GetBatchDetails(string reportId);
    }
}

