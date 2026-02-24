using EduTrackAcademics.Exception;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;

namespace EduTrackAcademics.Aspects
{
	public class ExceptionHandlerAttribute : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			var exceptionType = context.Exception.GetType();
			var message = context.Exception.Message;

			if (exceptionType == typeof(EnrollmentNotExistsException))
			{
				var result = new NotFoundObjectResult(message);
				context.Result = result;
			}
			else if (exceptionType == typeof(EnrollmentAlreadyExistsException))
			{
				var result = new ConflictObjectResult(message);
				context.Result = result;
			}
			else
			{
				var result = new StatusCodeResult(500);
				context.Result = result;
			}
		}
	}
}