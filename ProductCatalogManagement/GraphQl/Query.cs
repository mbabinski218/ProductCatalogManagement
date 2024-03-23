using HotChocolate.Data;
using MongoDB.Driver;
using MongoDbCollectionsGenerator;
using ProductCatalogManagement.Infrastructure.Collections;

namespace ProductCatalogManagement.GraphQl;

public class Query
{
	[UsePaging]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public IExecutable<Catalog> GetCatalogs([Service] IDbContext dbContext) 
		=> dbContext.GetCollection<Catalog>().AsExecutable();
	
	[UsePaging]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public IExecutable<Product> GetProducts([Service] IDbContext dbContext) 
		=> dbContext.GetCollection<Product>().AsExecutable();
	
	[UseSingleOrDefault]
	public IExecutable<Product> GetProductsById([Service] IDbContext dbContext, [ID] Guid id) 
		=> dbContext.GetCollection<Product>()
			.Find(x => x.Id == id)
			.AsExecutable();
	
	[UsePaging]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public IExecutable<Product> GetProductsByCatalogId([Service] IDbContext dbContext, Guid catalogId) 
		=> dbContext.GetCollection<Product>()
			.Find(x => x.CatalogId == catalogId)
			.AsExecutable();
	
	[UsePaging]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public IExecutable<User> GetUsers([Service] IDbContext dbContext) 
		=> dbContext.GetCollection<User>().AsExecutable();

	[UseSingleOrDefault]
	public IExecutable<User> GetUserById([Service] IDbContext dbContext, [ID] Guid id)
		=> dbContext.GetCollection<User>()
			.Find(x => x.Id == id)
			.AsExecutable();
}