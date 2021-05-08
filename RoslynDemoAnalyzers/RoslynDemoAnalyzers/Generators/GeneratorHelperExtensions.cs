using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynDemoAnalyzers.Generators
{
    public static class GeneratorHelperExtensions
    {
        public static void ArgumentNotNull(this object o, string argument)
        {
            if (o == null) throw new ArgumentNullException(argument);
        }
        public static string ToIdentifierSafeString(this string s)
        {
            if (string.IsNullOrEmpty(s)) throw new ArgumentNullException(nameof(s));

            if (char.IsDigit(s[0]))
            {
                s = "_" + s;
            }

            return s.Replace(" ", "");        
        }

        public static bool IsNumeric(this string s)
        {
            return long.TryParse(s, out var res);
        }
    }
}
