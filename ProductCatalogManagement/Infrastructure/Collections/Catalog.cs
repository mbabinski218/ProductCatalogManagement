using MongoDbCollectionsGenerator;
using ProductCatalogManagement.GraphQl.Resolvers;

namespace ProductCatalogManagement.Infrastructure.Collections;

[Collection("catalogs")]
[Node(
	IdField = nameof(Id),
	NodeResolverType = typeof(CatalogNodeResolver),
	NodeResolver = nameof(CatalogNodeResolver.ResolveAsync)
)]
public class Catalog
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
}