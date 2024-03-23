﻿using MongoDbCollectionsGenerator;
using ProductCatalogManagement.GraphQl.Resolvers;

namespace ProductCatalogManagement.Infrastructure.Collections;

[Collection("users")]
[Node(
	IdField = nameof(Id),
	NodeResolverType = typeof(UserNodeResolver),
	NodeResolver = nameof(UserNodeResolver.ResolveAsync)
)]
public class User
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string Email { get; set; } = null!;
}