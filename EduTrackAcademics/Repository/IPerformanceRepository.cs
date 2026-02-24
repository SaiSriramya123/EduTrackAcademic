using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
    public interface IPerformanceRepository
    {
        decimal GetCompletionPercentage(int enrollmentId);
        decimal GetAverageScore(int enrollmentId);
        DateTime GetLastModifiedDate(int enrollmentId);
        List<Performance> GetInstructorBatches(int instructorId);
        List<Performance> GetBatchPerformance(int batchId);
    }
}
