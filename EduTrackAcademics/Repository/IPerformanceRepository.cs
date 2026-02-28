using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using System.Collections.Generic;
namespace EduTrackAcademics.Repository
{
    public interface IPerformanceRepository
    {
        
        decimal GetAverageScore(string studentId);
        BatchPerformanceDTO GetLastModifiedDate(string enrollmentId);
        List<InstructorBatchDTO>GetInstructorBatches(string  instructorId);
        List<BatchPerformanceDTO> GetBatchPerformance(string  batchId);
       

    }
}
