using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VerifyCS = RoslynDemoAnalyzers.Test.CSharpCodeFixVerifier<RoslynDemoAnalyzers.IsAsyncMethodAsyncAnalyzer, RoslynDemoAnalyzers.IsAsyncMethodAsyncCodeFixProvider>;

namespace RoslynDemoAnalyzers.Test
{
    [TestClass]
    public class AsyncShouldBeAsyncTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task NoErrors() 
        {  
            var testCode = File.ReadAllText(@"TestFiles\Async\correct.cs");
             
            await VerifyCS.VerifyAnalyzerAsync(testCode);
        }

        [TestMethod]
        public async Task AsyncMethodShouldBeAsync()
        {
            var wrongCode = File.ReadAllText(@"TestFiles\Async\error.cs");
            var correctCode = File.ReadAllText(@"TestFiles\Async\fixed.cs");
                
            var expectedDiagnostic = VerifyCS.Diagnostic(DiagnosticIDs.IsAsyncMethodAsync).WithArguments("DoSomethingAsync").WithSpan(9,21,9,37);

            var test = new VerifyCS.Test
            {
                TestCode = wrongCode,
                FixedCode = correctCode
            };

            test.CompilerDiagnostics = CompilerDiagnostics.None;
            test.ExpectedDiagnostics.Add(expectedDiagnostic);

            await test.RunAsync(CancellationToken.None);


        }


    }
}
