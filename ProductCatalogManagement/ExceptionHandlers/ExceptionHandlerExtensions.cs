namespace ProductCatalogManagement.ExceptionHandlers;

public static class ExceptionHandlerExtensions
{
	public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
	{
		services.AddExceptionHandler<ExceptionHandler>();
		services.AddExceptionHandler<TimeOutExceptionHandler>();
		services.AddExceptionHandler<NotFoundExceptionHandler>();

		return services;
	}
	
	public static IApplicationBuilder UseExceptionHandlers(this IApplicationBuilder app)
	{
		app.UseExceptionHandler(_ => { });
		
		return app;
	}
}