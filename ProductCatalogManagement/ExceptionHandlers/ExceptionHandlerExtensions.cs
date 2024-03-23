namespace ProductCatalogManagement.ExceptionHandlers;

public static class ExceptionHandlerExtensions
{
	public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
	{
		services.AddExceptionHandler<ExceptionHandler>();
		services.AddExceptionHandler<TimeOutExceptionHandler>();

		return services;
	}
}