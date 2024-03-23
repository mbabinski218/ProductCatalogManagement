using MongoDB.Driver;
using MongoDbCollectionsGenerator;
using ProductCatalogManagement.Infrastructure.Collections;

namespace ProductCatalogManagement.GraphQl.Resolvers;

public class ProductNodeResolver
{
	public Task<Product> ResolveAsync([Service] IDbContext dbContext, Guid id)
		=> dbContext.GetCollection<Product>()
			.Find(x => x.Id == id)
			.FirstOrDefaultAsync();
}