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
    public class OneClassPerFileAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticID = DiagnosticIDs.OneClassPerFile;

        private static readonly string Title = "Only one class per file is allowed";
        private static readonly string MessageFormat = "Move '{0}' to it's own file, '{1}' has more than one class declared";
        private static readonly string Description = "Only allow one clas per file.";

        private const string Category = "Usage";

#pragma warning disable RS2008 // Enable analyzer release tracking
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticID, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);
#pragma warning restore RS2008 // Enable analyzer release tracking

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
        }

        private void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var root = context.Tree.GetRoot();
            var topLevelClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Where(i => i.Parent.Kind() == SyntaxKind.NamespaceDeclaration);
            if (topLevelClasses.Count() > 1)
            {
                foreach (var c in topLevelClasses.Skip(1))
                {
                    var diagnostic = Diagnostic.Create(Rule, c.Identifier.GetLocation(), c.Identifier.ValueText, context.Tree.FilePath);
                    context.ReportDiagnostic(diagnostic);
                }
            }

            
        }

    }
}
