namespace ProductCatalogManagement.Types;

public record UpdateProduct
(
	Guid Id,
	string Description,
	decimal Price,
	List<string> Categories
);