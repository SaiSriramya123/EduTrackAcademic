using EduTrackAcademics.Model;

namespace EduTrackAcademics.Services
{
    public interface IPerformanceService
    {
        decimal GetAverageScore(int enrollmentId);
        decimal GetCompletionPercentage(int enrollmentId);
        DateTime GetLastModifiedDate(int enrollmentId);
        List<Performance> GetInstructorBatches(int instructorId);
        List<Performance> GetBatchPerformance(int batchId);
    }
}
