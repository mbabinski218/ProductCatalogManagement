using System.Collections.Immutable;
using System.Text;

namespace MongoDbCollectionsGenerator;

public static class Helper
{
	public static string DbContextInterface => """
				using MongoDB.Driver;

				namespace MongoDbCollectionsGenerator
				{
					public interface IDbContext
					{
						IMongoDatabase Database { get; }
						IMongoCollection<T> GetCollection<T>();
						void Seed();
					}
				}
				""";
	
	public static string CollectionAttribute => """
				namespace MongoDbCollectionsGenerator;

				#pragma warning disable CS9113 // Parameter is unread.
				[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
				public sealed class CollectionAttribute(string name) : Attribute
				{
				}
				#pragma warning restore CS9113 // Parameter is unread.
				""";
	
	public static string GeneratePartialClass((string namespaceName, string className) dbContext, ImmutableArray<(string name, string type)?> collections)
	{
		var sb = new StringBuilder();
		sb.AppendLine("using MongoDB.Driver;");
		sb.AppendLine();
		sb.AppendLine($"namespace {dbContext.namespaceName}");
		sb.AppendLine("{");
		sb.AppendLine($"\tpublic partial class {dbContext.className}");
		sb.AppendLine("\t{");
		sb.AppendLine("\t\tpublic IMongoCollection<T> GetCollection<T>()");
		sb.AppendLine("\t\t{");
		sb.AppendLine("\t\t\treturn typeof(T).Name switch");
		sb.AppendLine("\t\t\t{");
		foreach (var collection in collections)
		{
			if (collection is not null)
			{
				sb.AppendLine($"\t\t\t\t\"{collection.Value.type}\" => Database.GetCollection<T>(\"{collection.Value.name}\"),");
			}
		}
		sb.AppendLine("\t\t\t\t_ => throw new InvalidOperationException($\"The class {typeof(T).Name} does not have a CollectionAttribute defined.\")");
		sb.AppendLine("\t\t\t};");
		sb.AppendLine("\t\t}");
		sb.AppendLine();
		sb.AppendLine("\t\tpublic void Seed()");
		sb.AppendLine("\t\t{");
		foreach (var collection in collections)
		{
			if (collection is not null)
			{
				sb.AppendLine($"\t\t\tDatabase.CreateCollection(\"{collection.Value.name}\");");
			}
		}
		sb.AppendLine("\t\t}");
		sb.AppendLine("\t}");
		sb.AppendLine("}");

		return sb.ToString();
	}
}