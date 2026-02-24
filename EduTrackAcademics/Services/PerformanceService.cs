using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using static EduTrackAcademics.Services.PerformanceService;

namespace EduTrackAcademics.Services
{
   
        public class PerformanceService : IPerformanceService
        {
            private readonly IPerformanceRepository _repo;
            public PerformanceService(IPerformanceRepository repo)
            {
                _repo = repo;
            }
            public decimal GetAverageScore(int enrollmentId)
            {
                return _repo.GetAverageScore(enrollmentId);
            }

            public decimal GetCompletionPercentage(int enrollmentId)
            {
                return _repo.GetCompletionPercentage(enrollmentId);

            }
            public DateTime GetLastModifiedDate(int enrollmentId)
            {
                return _repo.GetLastModifiedDate(enrollmentId);
            }
            public List<Performance> GetInstructorBatches(int instructorId)
            {
                return _repo.GetInstructorBatches(instructorId);
            }
            public List<Performance> GetBatchPerformance(int batchId)
            {
                return _repo.GetBatchPerformance(batchId);
            }
        }
    }


        

