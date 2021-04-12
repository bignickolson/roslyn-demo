using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using VerifyCS = RoslynDemoAnalyzers.Test.CSharpAnalyzerVerifier<RoslynDemoAnalyzers.BaseMethodShouldBeCalledAnalyzer>;

namespace RoslynDemoAnalyzers.Test
{
    [TestClass]
    public class BaseMethodShouldBeCalledTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task NoErrors()
        { 
            var testCode = File.ReadAllText(@"TestFiles\BaseMethodCalled\correct.cs");

            await VerifyCS.VerifyAnalyzerAsync(testCode);
        }

        [TestMethod]
        public async Task BaseMethodShouldBeCalled()
        {
            var testCode = File.ReadAllText(@"TestFiles\BaseMethodCalled\error.cs");
             
            var expectedDiagnostic = VerifyCS.Diagnostic(DiagnosticIDs.BaseMethodShouldBeCalled).WithArguments("DoSomething").WithSpan(11,30,11,41);
            await VerifyCS.VerifyAnalyzerAsync(testCode, expectedDiagnostic);
        }


    }
}
