using MongoDbCollectionsGenerator;
using ProductCatalogManagement.Infrastructure.Collections;
using ProductCatalogManagement.Types;

namespace ProductCatalogManagement.GraphQl;

public class Mutation
{
	public async Task<Catalog> CreateCatalog([Service] IDbContext dbContext, CreateCatalog catalogInput)
	{
		var catalog = new Catalog
		{
			Name = catalogInput.Name,
			Description = catalogInput.Description
		};
		
		await dbContext.GetCollection<Catalog>().InsertOneAsync(catalog);
		return catalog;
	}
}