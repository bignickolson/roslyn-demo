using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynDemoAnalyzers.Generators
{
    [Generator]
    public class SimpleGenerator : BaseGenerator
    {

        public override void Generate(GeneratorExecutionContext context)
        {
            context.AddSource("myGeneratedFile.cs", SourceText.From(@"
namespace GeneratedNamespace
{
    public class GeneratedClass
    {
        public static void GeneratedMethod()
        {
            // generated code
        }
    }
}", Encoding.UTF8));
        }


        public override void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
