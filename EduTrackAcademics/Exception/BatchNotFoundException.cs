namespace EduTrackAcademics.Exception
{
<<<<<<< HEAD
    
    
        public class BatchNotFoundException : ApplicationException
        {
            public int StatusCode { get; }
            public BatchNotFoundException(string message, int statusCode = 404)
                : base(message)
            {
                StatusCode = statusCode;
            }
        }
    
}
=======
	public class BatchNotFoundException : ApplicationException
	{
		public BatchNotFoundException(string batchId)
			: base($"Batch not found with id : {batchId}")
		{
		}
	}
}
>>>>>>> 4ebfee0d59e91bb63d0438af5366c1be4c80059b
