using MongoDB.Driver;
using MongoDbCollectionsGenerator;
using ProductCatalogManagement.Infrastructure.Collections;

namespace ProductCatalogManagement.GraphQl.Resolvers;

public class CatalogNodeResolver
{
	public Task<Catalog> ResolveAsync([Service] IDbContext dbContext, Guid id)
		=> dbContext.GetCollection<Catalog>()
			.Find(x => x.Id == id)
			.FirstOrDefaultAsync();
}