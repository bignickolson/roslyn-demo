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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ModelShouldEndWithModelCodeFixProvider)), Shared]
    public class ModelShouldEndWithModelCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(ModelShouldEndWithModelAnalyzer.DiagnosticID); }
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
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.ModelShouldEndWithModelCodeFixTitle,
                    createChangedSolution: c => AppendModel(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.ModelShouldEndWithModelCodeFixTitle)),
                diagnostic);

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.RemoveBaseModelTitle, 
                    createChangedDocument: c => RemoveBaseModel(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.RemoveBaseModelTitle)),
                 diagnostic); 
        }

        private async Task<Solution> AppendModel(Document document, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        {
            // Compute new uppercase name.
            var identifierToken = typeDecl.Identifier;
            var newName = identifierToken.Text + "Model";

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDecl, cancellationToken);

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return newSolution;
        }

        private async Task<Document> RemoveBaseModel(Document document, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        {
            var newDecl = typeDecl.RemoveNode(typeDecl.BaseList, SyntaxRemoveOptions.KeepEndOfLine);
            var root = await document.GetSyntaxRootAsync();
            var newRoot = root.ReplaceNode(typeDecl, newDecl);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
