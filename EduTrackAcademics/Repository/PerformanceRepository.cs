using System;
using System.Collections.Generic;
using System.Linq;
using EduTrackAcademics.Data;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Repository
{
    public class PerformanceRepository : IPerformanceRepository
    {
        private readonly EduTrackAcademicsContext _context;
        public PerformanceRepository(EduTrackAcademicsContext context)
        {
            _context = context;
        }

        // private readonly DummyPerformance _dummy = new();

        /* public decimal GetCompletionPercentage(int enrollmentId)
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

         }*/
        //method for AvgScore
        public decimal GetAverageScore(int enrollmentId)
{
   var avgScore =
       (from e in _context.Enrollments
        join s in _context.Students on e.StudentId equals s.StudentId
        join c in _context.Courses on e.CourseId equals c.CourseId
        join a in _context.Assessments on c.CourseId equals a.CourseID
        where e.EnrollmentId == enrollmentId
        select a.MarksObtained
       ).Average();
   return avgScore;
}


        //method for BatchPerformance
        public List<StudentBatchAssignment> GetBatchPerformance(string batchId)
        {
            return _context.StudentBatchAssignments
                           .Where(sba => sba.BatchId == batchId)
                           .ToList();
        }

        //method for LastModifiedDate
        public DateTime GetLastModifiedDate(int enrollmentId)
        {
            
            var studentId = _context.Performances
                .Where(p => p.EnrollmentId == enrollmentId)
                .Select(p => p.StudentId)
                .FirstOrDefault();
           
            if (studentId == null)
                return default(DateTime);
            
            var lastLogin = _context.StudentLoginHistories
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.LoginTime)
                .Select(s => s.LoginTime)
                .FirstOrDefault();
            return lastLogin;
        }



        public List<CourseBatch> GetInstructorBatches(string instructorId)
        {
            var result =
                (from cb in _context.CourseBatches where cb.InstructorId==instructorId
                 select cb).ToList();
            return result;
        }
       /* public List<Performance> GetBatchPerformance(int batchId)
        {
            var result =
                (from p in _dummy.Performances
                 where p.BatchId == batchId
                 select p)
                 .ToList();
            return result;*/


        }
    }


