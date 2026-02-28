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

			// ✅ EXISTING CODE – UNCHANGED
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

			// 🔹 ADDED: Course Exception
			else if (exceptionType == typeof(CourseNotFoundException))
			{
				var result = new NotFoundObjectResult(message);
				context.Result = result;
			}

			// 🔹 ADDED: Student Exception
			else if (exceptionType == typeof(StudentNotFoundException))
			{
				var result = new NotFoundObjectResult(message);
				context.Result = result;
			}

			// 🔹 ADDED: Batch Exception
			else if (exceptionType == typeof(BatchNotFoundException))
			{
				var result = new NotFoundObjectResult(message);
				context.Result = result;
			}

			// 🔹 ADDED: Instructor Exception
			else if (exceptionType == typeof(InstructorNotFoundException))
			{
				var result = new NotFoundObjectResult(message);
				context.Result = result;
			}

			// ✅ EXISTING DEFAULT – UNCHANGED
			else
			{
				var result = new StatusCodeResult(500);
				context.Result = result;
			}
		}
	}
}