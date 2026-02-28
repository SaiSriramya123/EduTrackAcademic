using System.Net;
using System.Text.Json;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EduExceptions = EduTrackAcademics.Exception;

namespace EduTrackAcademics.Aspects
{
	public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (System.Exception ex)
			{
				_logger.LogError(ex, "An unhandled exception occurred.");
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
		{
			context.Response.ContentType = "application/json";

			// Default to 500 Internal Server Error
			var statusCode = (int)HttpStatusCode.InternalServerError;
			var message = "An unexpected error occurred.";

			// Logic from your ExceptionHandlerAttribute
			if (exception is EduExceptions.EnrollmentNotExistsException)
			{
				statusCode = (int)HttpStatusCode.NotFound;
				message = exception.Message;
			}
			else if (exception is EduExceptions.EnrollmentAlreadyExistsException)
			{
				statusCode = (int)HttpStatusCode.Conflict;
				message = exception.Message;
			}
			// Add custom logic for Auth errors if needed
			else if (exception is System.UnauthorizedAccessException)
			{
				statusCode = (int)HttpStatusCode.Unauthorized;
				message = "You are not authorized to perform this action.";
			}

			else if (exception is InvalidEmailDomainException)
			{
				statusCode = (int)HttpStatusCode.BadRequest;
				message = exception.Message;
			}
			else if (exception is EmailAlreadyRegisteredException)
			{
				statusCode = (int)HttpStatusCode.Conflict;
				message = exception.Message;
			}


			context.Response.StatusCode = statusCode;

			var response = new ErrorResponse
			{
				StatusCode = statusCode,
				Message = message,
				// Only show stack trace in Development mode for security
				Details = _env.IsDevelopment() ? exception.StackTrace : null
			};

			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
			var json = JsonSerializer.Serialize(response, options);

			await context.Response.WriteAsync(json);
		}
	}
}




//public class GlobalExceptionMiddleware : ExceptionFilterAttribute
//{
//	public override void OnException(ExceptionContext context)
//	{
//		var exceptionType = context.Exception.GetType();
//		var message = context.Exception.Message;

//		if (exceptionType == typeof(EnrollmentNotExistsException))
//		{
//			var result = new NotFoundObjectResult(message);
//			context.Result = result;
//		}
//		else if (exceptionType == typeof(EnrollmentAlreadyExistsException))
//		{
//			var result = new ConflictObjectResult(message);
//			context.Result = result;
//		}
//		else
//		{
//			var result = new StatusCodeResult(500);
//			context.Result = result;
//		}
//	}
//}