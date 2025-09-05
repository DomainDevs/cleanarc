using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generators;

[Generator]
public class MapperGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MapperSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not MapperSyntaxReceiver receiver)
            return;

        foreach (var classDecl in receiver.CandidateClasses)
        {
            var model = context.Compilation.GetSemanticModel(classDecl.SyntaxTree);
            if (model.GetDeclaredSymbol(classDecl) is not INamedTypeSymbol classSymbol)
                continue;

            var attribute = classSymbol
                .GetAttributes()
                .FirstOrDefault(a => a.AttributeClass?.Name == "GenerateMapperAttribute");

            if (attribute is null)
                continue;

            var targetType = attribute.ConstructorArguments.First().Value as INamedTypeSymbol;
            if (targetType is null)
                continue;

            var source = GenerateMapperCode(classSymbol, targetType);
            context.AddSource($"{classSymbol.Name}_Mapper.g.cs", source);
        }
    }

    private string GenerateMapperCode(INamedTypeSymbol sourceType, INamedTypeSymbol targetType)
    {
        var sb = new StringBuilder();

        sb.AppendLine("using System;");
        sb.AppendLine();
        sb.AppendLine($"namespace {sourceType.ContainingNamespace}");
        sb.AppendLine("{");
        sb.AppendLine($"    public static partial class {sourceType.Name}Mapper");
        sb.AppendLine("    {");
        sb.AppendLine($"        public static {targetType.Name} To{targetType.Name}(this {sourceType.Name} source)");
        sb.AppendLine("        {");
        sb.AppendLine($"            return new {targetType.Name}");
        sb.AppendLine("            {");

        foreach (var prop in sourceType.GetMembers().OfType<IPropertySymbol>())
        {
            var targetProp = targetType.GetMembers().OfType<IPropertySymbol>()
                .FirstOrDefault(p => p.Name == prop.Name && SymbolEqualityComparer.Default.Equals(p.Type, prop.Type));

            if (targetProp != null)
            {
                sb.AppendLine($"                {targetProp.Name} = source.{prop.Name},");
            }
        }

        sb.AppendLine("            };");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }
}

class MapperSyntaxReceiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax cds && cds.AttributeLists.Count > 0)
        {
            CandidateClasses.Add(cds);
        }
    }
}
