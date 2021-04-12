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
    public class BaseMethodShouldBeCalledAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticID = DiagnosticIDs.BaseMethodShouldBeCalled;

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.BaseMethodShouldBeCalledAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.BaseMethodShouldBeCalledAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.BaseMethodShouldBeCalledAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticID, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {

            var methodSymbol = (IMethodSymbol)context.Symbol;

            var baseType = context.Compilation.GetTypeByMetadataName("TargetConsoleApp.Models.BaseModel");
            if (methodSymbol.ContainingType.InheritsFrom(baseType) 
                && methodSymbol.IsOverride
                && methodSymbol.Name == "DoSomething")
            {

                var syntax = methodSymbol.DeclaringSyntaxReferences[0].GetSyntax() as MethodDeclarationSyntax;
                var baseCalls = syntax.DescendantNodes().OfType<ExpressionStatementSyntax>().Where(i => i.ToString() == "base.DoSomething();");
                if (!baseCalls.Any())
                {
                    var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
