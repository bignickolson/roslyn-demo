using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RoslynDemoAnalyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(IsAsyncMethodAsyncCodeFixProvider)), Shared]
    public class IsAsyncMethodAsyncCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(IsAsyncMethodAsyncAnalyzer.DiagnosticID); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.AsyngMethodShouldBeAsyncTitle,
                    createChangedDocument: c => AddAsyncKeyword(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.AsyngMethodShouldBeAsyncTitle)),
                diagnostic);
        }

        private async Task<Document> AddAsyncKeyword(Document document, MethodDeclarationSyntax methodDecl, CancellationToken cancellationToken)
        {
            var newDecl = methodDecl.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
            var root = await document.GetSyntaxRootAsync();
            var newRoot = root.ReplaceNode(methodDecl, newDecl);

            return document.WithSyntaxRoot(newRoot);
        }

    }
}
