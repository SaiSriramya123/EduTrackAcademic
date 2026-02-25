using System;
using System.Collections.Generic;
using System.Linq;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
    public class PerformanceRepository : IPerformanceRepository
    {

        private readonly DummyPerformance _dummy = new();
      
        public decimal GetCompletionPercentage(int enrollmentId)
        {
            return (from p in _dummy.Performances
                    where p.EnrollmentId == enrollmentId
                    select p.CompletionPercentage).FirstOrDefault();

        }
        public decimal GetAverageScore(int enrollmentId)
        {
            return (from p in _dummy.Performances
                    where p.EnrollmentId == enrollmentId
                    select p.AvgScore).FirstOrDefault();
        }
        public DateTime GetLastModifiedDate(int enrollmentId)
        {
            return (from p in _dummy.Performances
                    where p.EnrollmentId == enrollmentId
                    select p.LastUpdated).FirstOrDefault();

        }
        public List<Performance> GetInstructorBatches(int instructorId)
        {
            var result =
                (from p in _dummy.Performances
                 where p.InstructorId == instructorId
                 select p)
                 .ToList();
            return result;
        }
        public List<Performance> GetBatchPerformance(int batchId)
        {
            var result =
                (from p in _dummy.Performances
                 where p.BatchId == batchId
                 select p)
                 .ToList();
            return result;


        }
    }
}

