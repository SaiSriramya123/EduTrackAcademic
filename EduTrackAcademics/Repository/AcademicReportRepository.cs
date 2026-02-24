using EduTrackAcademics.Dummy;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{


    public class AcademicReportRepository : IAcademicReportRepository

    {
        public List<AcademicReport> GetBatches()

        {

            return DummyAcademicReport.Reports.ToList();

        }

        public AcademicReport GetBatchDetails(string reportId)

        {
            return DummyAcademicReport.Reports

               .FirstOrDefault(r => r.ReportId == reportId);

        }

    }
}
 
    
