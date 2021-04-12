using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RoslynDemo.StandAlone
{
    public static class StandaloneAnalysis
    {
        public static async Task<int> DoAnalysis(Solution solution)
        {
            var title = $"Doing analysis of the solution: {solution.FilePath}";
            Console.WriteLine();
            Console.WriteLine(new string('-', title.Length));
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length));


            var result = 0;
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    var root = await document.GetSyntaxRootAsync();

                    if (CheckForDbRequiredAttribute(root))
                    {
                        result = 1;
                    }
                }
            }

            return result;
        }

        private static bool CheckForDbRequiredAttribute(SyntaxNode root)
        {
            var result = false;
            var attributes = root.DescendantNodes().OfType<AttributeSyntax>()
                .Where(i => i.Name.ToString().StartsWith("RequiresDatabaseRecord"));

            foreach (var attr in attributes)
            {
                var parentClass = GetParentClass(attr);
                if (parentClass != null)
                {
                    var location = parentClass.GetLocation().GetLineSpan();
                    // do a DB check here for something important
                    Console.WriteLine($"Class '{parentClass.Identifier.Text}' on line {location.StartLinePosition.Line} of file '{location.Path}' is missing a very important DB record");
                    result = true;
                }
            }
            return result;
        }

        private static ClassDeclarationSyntax GetParentClass(AttributeSyntax attr)
        {
            var p = attr.Parent;
            while (p != null)
            {
                if (p is ClassDeclarationSyntax)
                {
                    return p as ClassDeclarationSyntax;
                }

                p = p.Parent;
            }

            return null;
        }
    }
}
