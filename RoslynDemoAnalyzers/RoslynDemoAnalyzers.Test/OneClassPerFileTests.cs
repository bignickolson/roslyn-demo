using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VerifyCS = RoslynDemoAnalyzers.Test.CSharpAnalyzerVerifier<RoslynDemoAnalyzers.OneClassPerFileAnalyzer>;

namespace RoslynDemoAnalyzers.Test
{
    [TestClass]
    public class OneClassPerFileTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task NoErrors() 
        {  
            var testCode = File.ReadAllText(@"TestFiles\OneClass\correct.cs");
             
            await VerifyCS.VerifyAnalyzerAsync(testCode);
        }

        [TestMethod]
        public async Task MultipleClasses_NotAllowedTest()
        {
            var wrongCode = File.ReadAllText(@"TestFiles\OneClass\error.cs");

            var expectedDiagnostic = VerifyCS.Diagnostic(OneClassPerFileAnalyzer.DiagnosticID).WithArguments("SomeOtherClass","/0/Test0.cs").WithSpan(15,18,15,32);
            await VerifyCS.VerifyAnalyzerAsync(wrongCode, expectedDiagnostic);

        }

        [TestMethod]
        public async Task NestedClasses_Allowed()
        {
            var testCode = File.ReadAllText(@"TestFiles\OneClass\correctNested.cs");

            await VerifyCS.VerifyAnalyzerAsync(testCode);

        }


    }
}
