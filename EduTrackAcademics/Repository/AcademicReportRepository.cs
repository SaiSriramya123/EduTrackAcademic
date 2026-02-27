using EduTrackAcademics.Data;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Exception;
using EduTrackAcademics.Model;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Repository
{


    public class AcademicReportRepository : IAcademicReportRepository

    {

        private readonly EduTrackAcademicsContext  _context;
            public AcademicReportRepository(EduTrackAcademicsContext context)

        { 
            _context = context;

        }

        //method to get  batch reports
        public List<AcademicReport> GetReport()

        {

            var batches = _context.CourseBatches.ToList();

            if (batches == null || batches.Count == 0)

                throw new BatchNotFoundException("No batches available");

            var reports = new List<AcademicReport>();

            foreach (var batch in batches)

            {

                // Get students in batch

                var studentIds = _context.StudentBatchAssignments

                                    .Where(s => s.BatchId == batch.BatchId)

                                    .Select(s => s.StudentId)

                                    .ToList();

                if (studentIds.Count == 0)

                    continue;

                int totalStudents = studentIds.Count;

                // Get performance records

                var performances = _context.Performances

                                    .Where(p => studentIds.Contains(p.StudentId))

                                    .ToList();

                int completedStudents = performances

                                        .Where(p => p.AvgScore >= 40)

                                        .Count();

                decimal completionRate = totalStudents == 0

                    ? 0

                    : ((decimal)completedStudents / totalStudents) * 100;

                decimal avgScore = performances.Count == 0

                    ? 0

                    : performances.Average(p => p.AvgScore);

                int droppedStudents = totalStudents - completedStudents;

                decimal dropOutRate = totalStudents == 0

                    ? 0

                    : ((decimal)droppedStudents / totalStudents) * 100;

                // Check if report already exists

                var existingReport = _context.AcademicReport

                                        .FirstOrDefault(r => r.Scope == batch.BatchId);

                if (existingReport != null)

                {

                    // UPDATE

                    existingReport.CompletionRate = completionRate;

                    existingReport.AvgScore = avgScore;

                    existingReport.DropOutRate = dropOutRate;

                    existingReport.GeneratedDate = DateTime.Now;

                    reports.Add(existingReport);

                }

                else

                {

                    // INSERT

                    var newReport = new AcademicReport

                    {

                        ReportId = Guid.NewGuid().ToString(),

                        Scope = batch.BatchId,   // storing BatchId in Scope

                        CompletionRate = completionRate,

                        AvgScore = avgScore,

                        DropOutRate = dropOutRate,

                        GeneratedDate = DateTime.Now

                    };

                    _context.AcademicReport.Add(newReport);

                    reports.Add(newReport);

                }

            }

            _context.SaveChanges();

            return reports;

        }

        //method to get single batch report 

        public AcademicReport GetSingleReport(string batchId)

        {

            // Check batch exists

            var batch = _context.CourseBatches

                                .FirstOrDefault(b => b.BatchId == batchId);

            if (batch == null)

                throw new BatchNotFoundException("Batch not found");

            // Get students in this batch

            var studentIds = _context.StudentBatchAssignments

                                .Where(s => s.BatchId == batchId)

                                .Select(s => s.StudentId)

                                .ToList();

            if (studentIds.Count == 0)

                throw new NoStudentsFoundException("No students found in this batch");

            int totalStudents = studentIds.Count;

            // Get performances

            var performances = _context.Performances

                                .Where(p => studentIds.Contains(p.StudentId))

                                .ToList();

            int completedStudents = performances

                                    .Where(p => p.AvgScore >= 40)

                                    .Count();

            decimal completionRate = totalStudents == 0

                ? 0

                : ((decimal)completedStudents / totalStudents) * 100;

            decimal avgScore = performances.Count == 0

                ? 0

                : performances.Average(p => p.AvgScore);

            int droppedStudents = totalStudents - completedStudents;

            decimal dropOutRate = totalStudents == 0

                ? 0

                : ((decimal)droppedStudents / totalStudents) * 100;

            // Check if report already exists

            var existingReport = _context.AcademicReport

                                    .FirstOrDefault(r => r.Scope == batchId);

            if (existingReport != null)

            {

                // UPDATE

                existingReport.CompletionRate = completionRate;

                existingReport.AvgScore = avgScore;

                existingReport.DropOutRate = dropOutRate;

                existingReport.GeneratedDate = DateTime.Now;

                _context.SaveChanges();

                return existingReport;

            }

            else

            {

                // INSERT

                var newReport = new AcademicReport

                {

                    ReportId = Guid.NewGuid().ToString(),

                    Scope = batchId,   // storing BatchId in Scope

                    CompletionRate = completionRate,

                    AvgScore = avgScore,

                    DropOutRate = dropOutRate,

                    GeneratedDate = DateTime.Now

                };

                _context.AcademicReport.Add(newReport);

                _context.SaveChanges();

                return newReport;

            }

        }

    }
}





    

