using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGeneratorFoo;

[Generator(LanguageNames.CSharp)]
public class CodeCollectionIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ClassDeclarationSyntax> provider = context.SyntaxProvider
            .CreateSyntaxProvider((s, t) => s is ClassDeclarationSyntax,
                (ctx, t) =>
                {
                    var classDeclarationSyntax = ctx.Node as ClassDeclarationSyntax;
                    if (classDeclarationSyntax == null) 
                        return (classDeclarationSyntax, false);
                    if (classDeclarationSyntax.Identifier.Text != "Program")
                        return (classDeclarationSyntax, false);
                    return (classDeclarationSyntax, true);
                })
            .Where(t => t.Item2)
            .Select((t, token) => t.Item1!);
        
        context.RegisterSourceOutput(provider, (ctx, syntax) =>
        {
            string source = @"
using System;

namespace SourceGeneratorFoo.Sample;

public static partial class Program
{
    static partial void HelloFrom(string name)
    {
        Console.WriteLine($""Says: Hi from '{name}'"");
    }
}
";
            ctx.AddSource("GeneratedSourceTest", SourceText.From(source, Encoding.UTF8));

        });
    }
}