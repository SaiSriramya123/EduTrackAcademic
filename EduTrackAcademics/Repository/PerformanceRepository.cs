using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Exception;
using EduTrackAcademics.Model;
using Microsoft.EntityFrameworkCore;
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


        //method for getbatchperformance
        public List<BatchPerformanceDTO> GetBatchPerformance(string batchId)

        {

            var students = (from sba in _context.StudentBatchAssignments

                            join s in _context.Student on sba.StudentId equals s.StudentId

                            join cb in _context.CourseBatches on sba.BatchId equals cb.BatchId

                            join c in _context.Course on cb.CourseId equals c.CourseId

                            where cb.BatchId == batchId

                            select new

                            {

                                s.StudentId,

                                s.StudentName,

                                c.CourseName

                            }).ToList();

            var result = new List<BatchPerformanceDTO>();

            foreach (var student in students)

            {

                // 🔹 Latest Marks

                var latestMarks = (from sub in _context.Submissions

                                   where sub.StudentID == student.StudentId

                                   orderby sub.SubmissionDate descending

                                   select sub.Score).FirstOrDefault();

                // 🔹 Average Score

                var avgScore = (from sub in _context.Submissions

                                where sub.StudentID == student.StudentId

                                select (decimal?)sub.Score).Average() ?? 0;

                // 🔹 Get Performance row

                var performance = (from p in _context.Performances

                                   where p.StudentId == student.StudentId

                                   select p).FirstOrDefault();

                if (performance != null)

                {

                    performance.AvgScore = avgScore;

                    performance.LastUpdated = DateTime.Now;

                }

                result.Add(new BatchPerformanceDTO

                {

                    StudentId = student.StudentId,

                    StudentName = student.StudentName,

                    CourseName = student.CourseName,

                    Marks = latestMarks,

                    AvgScore = avgScore

                });

            }

            _context.SaveChanges();

            return result;

        }


        //method for AvgScore
        public decimal GetAverageScore(string studentId)
        {
            //  Calculate Average Score
            var avgScore = (from s in _context.Submission
                            where s.StudentID == studentId
                            select (decimal?)s.Score).Average() ?? 0;
            //  Get Performance record
            var performance = (from p in _context.Performances
                               where p.StudentId == studentId
                               select p).FirstOrDefault();
            if (performance == null)
                throw new StudentNotFoundException("Performance record not found", 404);
            //  Update AvgScore column
            performance.AvgScore = avgScore;
            //  Save changes
            _context.SaveChanges();
            return avgScore;
        }


        //method for LastModifiedDate
        public BatchPerformanceDTO GetLastModifiedDate(string enrollmentId)
        {
            var data = (from p in _context.Performances
                        join s in _context.Student on p.StudentId equals s.StudentId
                        join b in _context.CourseBatches on p.BatchId equals b.BatchId
                        join c in _context.Course on b.CourseId equals c.CourseId
                        where p.EnrollmentId == enrollmentId
                        select new
                        {
                            Performance = p,
                            StudentName = s.StudentName,
                            CourseName = c.CourseName
                        }).FirstOrDefault();
            if (data == null)
                throw new EnrollmentNotExistsException("Enrollment not found", 404);
            var lastLogin = (from sl in _context.StudentLoginHistories
                             where sl.StudentId == data.Performance.StudentId
                             orderby sl.LoginTime descending
                             select sl.LoginTime).FirstOrDefault();
            if (lastLogin == default(DateTime))
                throw new StudentNotFoundException("No login history found", 404);

            //  Update Performance Table
            data.Performance.LastUpdated = lastLogin;
            _context.SaveChanges();
            return new BatchPerformanceDTO
            {
                StudentName = data.StudentName,
                CourseName = data.CourseName,
                LastUpdated = lastLogin
            };
        }

        //method for getting Instructor Batches

        public List<InstructorBatchDTO> GetInstructorBatches(string instructorId)
        {
            var result = (from b in _context.CourseBatches
                          join c in _context.Course
                          on b.CourseId equals c.CourseId
                          where b.InstructorId == instructorId
                          select new InstructorBatchDTO
                          {
                              BatchId = b.BatchId,
                              InstructorId = b.InstructorId,
                              CourseName = c.CourseName
                          }).ToList();
            if (result.Count == 0)
                throw new InstructorBatchesNotFoundException("No batches found", 404);
            return result;
        }



    }
    }


