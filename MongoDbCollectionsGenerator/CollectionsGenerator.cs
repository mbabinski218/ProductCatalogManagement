using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MongoDbCollectionsGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class CollectionsGenerator : IIncrementalGenerator
{
    private const string databaseInterface = "IDbContext";
    private const string minCollectionAttribute = "CollectionAttribute";
    private const string fullCollectionAttribute = "MongoDbCollectionsGenerator.CollectionAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "IDbContext.g.cs", SourceText.From(Helper.DbContextInterface, Encoding.UTF8)));
            
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "CollectionAttribute.g.cs", SourceText.From(Helper.CollectionAttribute, Encoding.UTF8)));
            
        var dbContexts = context.SyntaxProvider
            .CreateSyntaxProvider
            (
                (node, _) => node is ClassDeclarationSyntax classSyntax && ImplementsInterface(classSyntax, databaseInterface),
                GetDbContextName
            );
            
        var collections = context.SyntaxProvider
            .ForAttributeWithMetadataName
            (
                fullCollectionAttribute,
                (node, _) => node is ClassDeclarationSyntax,
                GetCollectionNameAndType
            );
            
        var combined = dbContexts.Combine(collections.Collect());
            
        context.RegisterSourceOutput(combined, static (spc, combined) =>
        {
            if (combined.Left is null || combined.Right.Length <= 0)
            {
                return;
            }
                 
            var hintName = $"{combined.Left.Value.className}.g.cs";
            var result = Helper.GeneratePartialClass(combined.Left.Value, combined.Right);
                 
            spc.AddSource(hintName, SourceText.From(result, Encoding.UTF8));
        });
    }

    private static bool ImplementsInterface(BaseTypeDeclarationSyntax syntax, string interfaceName)
    {
        return syntax.BaseList?.Types.Any(t => t.Type is IdentifierNameSyntax identifier 
                                               && identifier.Identifier.Text == interfaceName) == true;
    }

    private static (string namespaceName, string className)?  GetDbContextName(GeneratorSyntaxContext context, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
            
        if (context.SemanticModel.GetDeclaredSymbol(context.Node, ct) is not INamedTypeSymbol symbol)
        {
            return null;
        }
            
        if (string.IsNullOrEmpty(symbol.Name))
        {
            return null;
        }
            
        var namespaceName = symbol.ContainingNamespace.ToDisplayString();
        var className = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

        return (namespaceName, className);
    }

    private static (string name, string type)? GetCollectionNameAndType(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        
        if (context.TargetSymbol is not INamedTypeSymbol symbol)
        {
            return null;
        }
            
        if (string.IsNullOrEmpty(symbol.Name))
        {
            return null;
        }

        var attributeData = symbol
            .GetAttributes()
            .First(x => x.AttributeClass?.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat) == minCollectionAttribute);
            
        var nameArgument = attributeData.ConstructorArguments.First();

        var name = nameArgument.Value as string;
            
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        var type = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

        return (name!, type);
    }
}