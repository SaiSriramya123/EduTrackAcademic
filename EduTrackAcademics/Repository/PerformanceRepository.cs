using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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

        //method fro getbatchperformance

        public List<BatchPerformanceDTO> GetBatchPerformance(string batchId)

        {

            var result =

                (from sba in _context.StudentBatchAssignments

                 join s in _context.Student

                    on sba.StudentId equals s.StudentId

                 join e in _context.Enrollment

                    on s.StudentId equals e.StudentId

                 join c in _context.Course

                    on e.CourseId equals c.CourseId

                 where sba.BatchId == batchId

                 select new BatchPerformanceDTO

                 {

                     StudentId = s.StudentId,

                     StudentName = s.StudentName,

                     CourseName = c.CourseName,

                     AvgScore = _context.Submissions

                        .Where(sub => sub.StudentID == s.StudentId)

                        .Select(sub => (decimal?)sub.Score)

                        .Average() ?? 0

                 }).ToList();

            return result;

        }

        //method for AvgScore
        public decimal GetAverageScore(string studentId)
        {
            var avgScore = _context.Submissions
                .Where(s => s.StudentID == studentId)
                .Select(s => (decimal?)s.Score)
                .Average() ?? 0;
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


