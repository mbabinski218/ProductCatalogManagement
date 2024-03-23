using MongoDB.Driver;
using MongoDbCollectionsGenerator;

namespace ProductCatalogManagement.Infrastructure;

public sealed class DatabaseManager : IDatabaseManager
{
	private readonly IDbContext _dbContext;
	
	public DatabaseManager(IDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	
	public void Seed()
	{
		if (_dbContext.Database.ListCollectionNames().ToList().Count == 0)
		{
			_dbContext.Seed();
		}
	}
}