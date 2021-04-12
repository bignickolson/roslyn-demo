using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using RoslynDemo.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace RoslynDemoAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IsAsyncMethodAsyncAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticID = DiagnosticIDs.IsAsyncMethodAsync;

        private static readonly string Title = "Async methods should be marked with the async keyword";
        private static readonly string MessageFormat = "Method '{0}' should be marked with the async keyword";
        private static readonly string Description = "Recommend methods named ...Async should have async keyword";

        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticID, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.MethodDeclaration);

            // You could register this to look at the whole tree of a document
            //context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var methodSyntax = context.Node as MethodDeclarationSyntax;
            if (methodSyntax.Identifier.ValueText.EndsWith("Async")
                && !methodSyntax.Modifiers.Any(SyntaxKind.AsyncKeyword))
            {
                var diagnostic = Diagnostic.Create(Rule, methodSyntax.Identifier.GetLocation(), methodSyntax.Identifier.ValueText);
                context.ReportDiagnostic(diagnostic);
            }
        }

    }
}
