using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogManagement.Exceptions;

namespace ProductCatalogManagement.ExceptionHandlers;

public sealed class NotFoundExceptionHandler : IExceptionHandler
{
	private const string title = "Item not found";
	private const ushort statusCode = (ushort)HttpStatusCode.NotFound;
	
	private readonly ILogger<NotFoundExceptionHandler> _logger;
	
	public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		_logger.LogError(exception, title);

		if (exception is not NotFoundException)
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