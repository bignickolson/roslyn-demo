using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynDemoAnalyzers.Generators.Models
{
    public sealed class LookupValue
    {
        public LookupValue(string name, string value)
        {
            name.ArgumentNotNull(nameof(name));
            value.ArgumentNotNull(nameof(value));

            if (!value.IsNumeric())
            {
                throw new ArgumentException("Parameter must be a numeric value", nameof(value));
            }

            Name = name.ToIdentifierSafeString();
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}
