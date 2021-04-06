using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynDemoAnalyzers.Test.TestFiles.GrandChild
{
    public class {|#0:GrandChild|} : ChildModel
    {
    }

    public class ChildModel : TargetConsoleApp.Models.BaseModel
    { }
}

namespace TargetConsoleApp.Models
{
    public class BaseModel
    {
    }
}
