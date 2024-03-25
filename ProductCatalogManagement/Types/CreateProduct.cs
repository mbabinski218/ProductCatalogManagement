namespace ProductCatalogManagement.Types;

public record CreateProduct
(
	Guid CatalogId,
	string Name,
	decimal Price,
	string? Description,
	List<string> Categories
);