using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogManagement.ExceptionHandlers;

public sealed class TimeOutExceptionHandler : IExceptionHandler
{
	private const string title = "A timeout occurred";
	private const ushort statusCode = (ushort)HttpStatusCode.RequestTimeout;
	
	private readonly ILogger<TimeOutExceptionHandler> _logger;
	
	public TimeOutExceptionHandler(ILogger<TimeOutExceptionHandler> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		_logger.LogError(exception, title);

		if (exception is not TimeoutException)
		{
			return false;
		}

		httpContext.Response.StatusCode = statusCode;
		await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
		{
			Status = statusCode,
			Type = exception.GetType().Name,
			Title = title,
			Detail = exception.Message,
			Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
		}, cancellationToken);
			
		return true;
	}
}