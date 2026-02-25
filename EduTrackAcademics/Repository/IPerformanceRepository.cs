using EduTrackAcademics.Model;
using System.Collections.Generic;
namespace EduTrackAcademics.Repository
{
    public interface IPerformanceRepository
    {
        decimal GetCompletionPercentage(int enrollmentId);
        decimal GetAverageScore(int enrollmentId);
        DateTime GetLastModifiedDate(int enrollmentId);
        List<CourseBatch> GetInstructorBatches(string  instructorId);
        List<StudentBatchAssignment> GetBatchPerformance(string  batchId);
    }
}
