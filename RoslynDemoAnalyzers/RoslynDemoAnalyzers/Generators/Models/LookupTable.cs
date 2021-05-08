using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynDemoAnalyzers.Generators.Models
{
    public sealed class LookupTable
    {
        public LookupTable(string name)
        {
            name.ArgumentNotNull(nameof(name));

            Name = name.ToIdentifierSafeString();
        }
        public string Name { get; }
        public List<LookupValue> Values { get; set; } = new List<LookupValue>();
    }
}
