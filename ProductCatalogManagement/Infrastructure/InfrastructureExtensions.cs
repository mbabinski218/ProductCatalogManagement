using MongoDbCollectionsGenerator;

namespace ProductCatalogManagement.Infrastructure;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddOptions(configuration);
		services.AddScoped<IDbContext, DbContext>();
		services.AddScoped<IDatabaseManager, DatabaseManager>();
		
		return services;
	}
	
	private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
	{
		var databaseName = configuration.GetSection("DatabaseName").Value 
		    ?? throw new InvalidOperationException("DatabaseName is not defined in the appsettings.json file.");
		
		var connectionString = configuration.GetConnectionString(databaseName) 
		    ?? throw new InvalidOperationException($"Connection string for the database {databaseName} is not defined in the appsettings.json file.");
		
		var options = new DbContextOptions
		{
			DatabaseName = databaseName,
			ConnectionString = connectionString
		};
		
		services.AddSingleton(options);
		
		return services;
	}
	
	public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		var databaseManager = scope.ServiceProvider.GetRequiredService<IDatabaseManager>();
		databaseManager.Seed();
		
		return app;
	}
}