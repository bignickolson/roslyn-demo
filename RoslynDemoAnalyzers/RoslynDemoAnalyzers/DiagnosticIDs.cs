using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynDemoAnalyzers
{
    public static class DiagnosticIDs
    {
        public const string ModelShouldEndWithModel = "RoslynDemo001";
        public const string BaseMethodShouldBeCalled = "RoslynDemo002";
        public const string IsAsyncMethodAsync = "RoslynDemo003";
    }
}
