namespace EduTrackAcademics.Exception
{

	public class BatchNotFoundException : ApplicationException
	{
		public BatchNotFoundException(string batchId)
			: base($"Batch not found with id : {batchId}")
		{
		}
	}
}

