using EduTrackAcademics.Dummy;
using EduTrackAcademics.Services;

namespace EduTrackAcademics.Repository
{
    public class Coordinatorrepo:ICoordinatorrepo
    {
		private readonly DummyInstructor _dm;
		public Coordinatorrepo(DummyInstructor dm)
		{
			_dm = dm;
		}
		public List<string> GetInstructorData()
		{
			if (_dm.ApprovedInstructorEmails != null)
			{
				return _dm.ApprovedInstructorEmails.ToList();
			}
			return new List<string>();


		}

    }
}
