using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynDemo.Common
{
    public static class AnalyerExtensions
    {
        public static bool InheritsFrom(this ITypeSymbol symbol, ITypeSymbol expectedBaseType)
        {
            var baseType = symbol;
            while (baseType != null)
            {
                if (SymbolEqualityComparer.Default.Equals(baseType, expectedBaseType))
                {
                    return true;
                }
                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}
