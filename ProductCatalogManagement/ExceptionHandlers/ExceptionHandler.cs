using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogManagement.ExceptionHandlers;

public sealed class ExceptionHandler : IExceptionHandler
{
	private const string title = "An unexpected error occurred";
	private const ushort statusCode = (ushort)HttpStatusCode.InternalServerError;
	
	private readonly ILogger<ExceptionHandler> _logger;
	
	public ExceptionHandler(ILogger<ExceptionHandler> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		_logger.LogError(exception, title);

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