
namespace ProductCatalogManagement.GraphQl;

public static class GraphQlExtensions
{
	public static IServiceCollection AddGraphQl(this IServiceCollection services)
	{
		services.AddGraphQLServer()
			.AddQueryType<Query>()
			.AddMutationType<Mutation>()
			.AddGlobalObjectIdentification()
			.AddMongoDbPagingProviders()
			.AddMongoDbProjections()
			.AddMongoDbFiltering()
			.AddMongoDbSorting();
		
		return services;
	}

	public static WebApplication MapGraphQl(this WebApplication app)
	{
		app.MapGraphQL();
		
		return app;
	}
}