namespace EduTrackAcademics.Exception
{
	public class StudentNotFoundException : ApplicationException
	{
		public StudentNotFoundException(string id)
	   : base($"Student {id} not found")
		{
		}
	}
}
