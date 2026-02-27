using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using System.Collections.Generic;   
namespace EduTrackAcademics.Services
{
    public interface IPerformanceService
    {
        decimal GetAverageScore(string studentId);
       
        BatchPerformanceDTO GetLastModifiedDate(string enrollmentId);
        List < InstructorBatchDTO >GetInstructorBatches(string instructorId);
        List<BatchPerformanceDTO> GetBatchPerformance(string batchId);
        
    }
}
