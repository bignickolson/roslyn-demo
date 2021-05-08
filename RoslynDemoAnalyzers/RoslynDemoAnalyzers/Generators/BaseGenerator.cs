using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynDemoAnalyzers.Generators
{
    public abstract class BaseGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                Generate(context);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("/*");
                sb.AppendLine(ex.ToString());
                sb.AppendLine("*/");
                context.AddSource("ErrorDuringGeneration.cs", sb.ToString());
            }
        }

        public abstract void Generate(GeneratorExecutionContext context);

        public abstract void Initialize(GeneratorInitializationContext context);
    }
}
