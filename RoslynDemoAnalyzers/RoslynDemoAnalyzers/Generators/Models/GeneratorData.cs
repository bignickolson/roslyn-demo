using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynDemoAnalyzers.Generators.Models
{
    public sealed class GeneratorData
    {
        public GeneratorData(string defaultNamespace)
        {
            defaultNamespace.ArgumentNotNull(nameof(defaultNamespace));

            ProjectDefaultNamespace = defaultNamespace;

        }
        public string ProjectDefaultNamespace { get; set; }
        public List<LookupTable> LookupTables { get; set; } = new List<LookupTable>();

    }
}
