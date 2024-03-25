namespace ProductCatalogManagement.Exceptions;

public sealed class NotFoundException(string msg = "Item not found") : Exception(msg);