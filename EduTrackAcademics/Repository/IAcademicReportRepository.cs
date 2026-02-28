using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
    public interface IAcademicReportRepository
    {
        
        List<AcademicReport> GetReport();
        AcademicReport GetSingleReport(string batchId);
    }
}

