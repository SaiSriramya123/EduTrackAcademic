using EduTrackAcademics.Model;

namespace EduTrackAcademics.Dummy
{
    public class DummyPerformance
    {

        public List<Performance> Performances => new()
        {
            new Performance
            {
                EnrollmentId=1,
                AvgScore=75,
                CompletionPercentage=89,
                LastUpdated=DateTime.Now.AddDays(-1),
                BatchId=101,
                InstructorId=500
            },
             new Performance
            {
                EnrollmentId=2,
                AvgScore=85,
                CompletionPercentage=60,
                LastUpdated=DateTime.Now.AddDays(-2),
                  BatchId=102,
                InstructorId=890
            },
              new Performance
            {
                EnrollmentId=3,
                AvgScore=90,
                CompletionPercentage=95,
                LastUpdated=DateTime.Now,
                   BatchId=103,
                InstructorId=502
            }

        };
    }
}
