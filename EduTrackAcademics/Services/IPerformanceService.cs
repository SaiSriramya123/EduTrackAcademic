using EduTrackAcademics.Model;
using System.Collections.Generic;   
namespace EduTrackAcademics.Services
{
    public interface IPerformanceService
    {
        decimal GetAverageScore(int enrollmentId);
        decimal GetCompletionPercentage(int enrollmentId);
        DateTime GetLastModifiedDate(int enrollmentId);
        List<CourseBatch> GetInstructorBatches(string instructorId);
        List<Performance> GetBatchPerformance(int batchId);
    }
}
