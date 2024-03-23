using MongoDB.Driver;
using MongoDbCollectionsGenerator;

namespace ProductCatalogManagement.Infrastructure;

public sealed class DbContextOptions
{
	public string DatabaseName { get; init; } = null!;
	public string ConnectionString { get; init; } = null!;
}

public partial class DbContext : IDbContext
{
	public IMongoDatabase Database { get; }

	public DbContext(DbContextOptions options)
	{
		var client = new MongoClient(options.ConnectionString);
		Database = client.GetDatabase(options.DatabaseName);
	}
}