using MongoDB.Driver;
using MongoDbCollectionsGenerator;
using ProductCatalogManagement.Exceptions;
using ProductCatalogManagement.Infrastructure.Collections;
using ProductCatalogManagement.Types;
using ReturnDocument = MongoDB.Driver.ReturnDocument;

namespace ProductCatalogManagement.GraphQl;

public class Mutation
{
	public async Task<User> CreateUser([Service] IDbContext dbContext, CreateUser input)
	{
		var user = User.Create(input.FirstName, input.LastName, input.Email);
		await dbContext.GetCollection<User>().InsertOneAsync(user);
		return user;
	}
	
	public async Task<Catalog> CreateCatalog([Service] IDbContext dbContext, CreateCatalog input)
	{
		var catalog = Catalog.Create(input.Name, input.Description);
		await dbContext.GetCollection<Catalog>().InsertOneAsync(catalog);
		return catalog;
	}
	
	public async Task<Product> CreateProduct([Service] IDbContext dbContext, CreateProduct input)
	{
		var product = Product.Create(input.CatalogId, input.Name, input.Description, input.Price, input.Categories);
		await dbContext.GetCollection<Product>().InsertOneAsync(product);
		return product;
	}
	
	public async Task<Product> UpdateProduct([Service] IDbContext dbContext, UpdateProduct input)
	{
		var filters = Builders<Product>.Filter.Eq(x => x.Id, input.Id);
		
		var update = Builders<Product>.Update
			.Set(x => x.Description, input.Description)
			.Set(x => x.Price, input.Price)
			.Set(x => x.Categories, input.Categories);
		
		var options = new FindOneAndUpdateOptions<Product>
		{
			IsUpsert = false,
			ReturnDocument = ReturnDocument.Before
		};
		
		var product = await dbContext.GetCollection<Product>().FindOneAndUpdateAsync(filters, update, options);
		if (product is null)
		{
			throw new NotFoundException("Product not found.");
		}
		
		return product;
	}
}