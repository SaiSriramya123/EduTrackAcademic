using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;

namespace EduTrackAcademics.Services
{
    public class AcademicReportService : IAcademicReportService
    {
        private readonly IAcademicReportRepository _repo;
        public AcademicReportService(IAcademicReportRepository repo)
        {
            _repo = repo;
        }
        public List<AcademicReport> GetReport()
        {
            return _repo.GetReport();
        }
        public AcademicReport GetSingleReport(string batchId)
        {
            return _repo.GetSingleReport(batchId);
        }
    }
}