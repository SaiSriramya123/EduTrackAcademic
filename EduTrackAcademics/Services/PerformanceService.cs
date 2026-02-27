using EduTrackAcademics.DTO;
using EduTrackAcademics.Exception;
using EduTrackAcademics.Migrations;
using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using static EduTrackAcademics.Services.PerformanceService;

namespace EduTrackAcademics.Services
{

    public class PerformanceService : IPerformanceService
    {

        private readonly IPerformanceRepository _repo;


    
        public decimal GetAverageScore(string studentId)
        {
            return _repo.GetAverageScore(studentId);
        }

        public BatchPerformanceDTO GetLastModifiedDate(string enrollmentId)
        {
            return _repo.GetLastModifiedDate(enrollmentId);
        }

        public List <InstructorBatchDTO> GetInstructorBatches(string instructorId)
        {
            return _repo.GetInstructorBatches(instructorId);
        }

        public List<BatchPerformanceDTO> GetBatchPerformance(string batchId)
        {
            return _repo.GetBatchPerformance(batchId);
        }
    }
}