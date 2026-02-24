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
        public List<AcademicReport> GetBatches()
        {
            return _repo.GetBatches();
        }
        public AcademicReport GetBatchDetails(string reportId)
        {
            return _repo.GetBatchDetails(reportId);
        }
    }
}