using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetConsoleApp.Models.Attributes;

namespace TargetConsoleApp.Models
{ 
    public class ChildModel : BaseModel
    { 
        public override void DoSomething()
        {

            base.DoSomething();
            var x = 0;
            x++;
            "somestring".ToString();
        }

        //public Task DoSomethingAsync()
        //{
        //}

        public class NestedClass { }
    }

}
