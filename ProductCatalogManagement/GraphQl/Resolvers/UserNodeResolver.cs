using MongoDB.Driver;
using MongoDbCollectionsGenerator;
using ProductCatalogManagement.Infrastructure.Collections;

namespace ProductCatalogManagement.GraphQl.Resolvers;

public class UserNodeResolver
{
	public Task<User> ResolveAsync([Service] IDbContext dbContext, Guid id)
		=> dbContext.GetCollection<User>()
			.Find(x => x.Id == id)
			.FirstOrDefaultAsync();
}