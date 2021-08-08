using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

            var regex = new Regex("[^0-9a-zA-Z_]");
            s = regex.Replace(s, "");
            if (char.IsDigit(s[0]))
            {
                s = "_" + s;
            }

            return new string(regex.Replace(s, "").Skip(0).Take(512).ToArray());
        }

        public static bool IsNumeric(this string s)
        {
            return long.TryParse(s, out var res);
        }
    }
}
