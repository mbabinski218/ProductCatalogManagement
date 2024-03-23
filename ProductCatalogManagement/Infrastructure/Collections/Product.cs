using MongoDbCollectionsGenerator;
using ProductCatalogManagement.GraphQl.Resolvers;

namespace ProductCatalogManagement.Infrastructure.Collections;

[Collection("products")]
[Node(
	IdField = nameof(Id),
	NodeResolverType = typeof(ProductNodeResolver),
	NodeResolver = nameof(ProductNodeResolver.ResolveAsync)
)]
public class Product
{
	public Guid Id { get; set; }
	public Guid CatalogId { get; set; }
	public string Name { get; set; } = null!;
	public decimal Price { get; set; }
	public string? Description { get; set; }
	public List<string> Categories { get; set; } = [];
}