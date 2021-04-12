using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using VerifyCS = RoslynDemoAnalyzers.Test.CSharpCodeFixVerifier<
    RoslynDemoAnalyzers.ModelShouldEndWithModelAnalyzer,
    RoslynDemoAnalyzers.ModelShouldEndWithModelCodeFixProvider>;

namespace RoslynDemoAnalyzers.Test
{
    [TestClass]
    public class ModelShouldEndWithModelTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task NoErrors()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task Models_Should_EndWith_Model()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:Test|} : TargetConsoleApp.Models.BaseModel
        {   
        }
    }

    namespace TargetConsoleApp.Models
    {
      public class BaseModel {}  
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TestModel : TargetConsoleApp.Models.BaseModel
        {   
        }
    }

    namespace TargetConsoleApp.Models
    {
      public class BaseModel {}  
    }";

            var expected = VerifyCS.Diagnostic(DiagnosticIDs.ModelShouldEndWithModel).WithLocation(0).WithArguments("Test");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }

        [TestMethod]
        public async Task GrandChild_Model_Should_EndWith_Model()
        {
            var testCode = File.ReadAllText(@"TestFiles\GrandChild\code.cs");
            var fixedCode = File.ReadAllText(@"TestFiles\GrandChild\fixed.cs");

            // create the expected diagnostic
            var expectedDiagnostic = VerifyCS.Diagnostic(DiagnosticIDs.ModelShouldEndWithModel).WithLocation(0).WithArguments("GrandChild");

            // verify the diagnostic showed up and the code fix was applied
            await VerifyCS.VerifyCodeFixAsync(testCode, expectedDiagnostic, fixedCode);

        }

        [TestMethod]
        public async Task Remove_Model_Base()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:Test|} : TargetConsoleApp.Models.BaseModel
        {   
        }
    }

    namespace TargetConsoleApp.Models
    {
      public class BaseModel {}  
    }";

            // extra space after Test i can't seem to get rid of
            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class Test 
        {   
        }
    }

    namespace TargetConsoleApp.Models
    {
      public class BaseModel {}  
    }";

            var expected = VerifyCS.Diagnostic(DiagnosticIDs.ModelShouldEndWithModel).WithLocation(0).WithArguments("Test");

            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest, 1);
        }
    }
}
