using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetConsoleApp.Models.Attributes;

namespace TargetConsoleApp.Models
{ 
    [RequiresDatabaseRecord]
    public class Child : BaseModel
    { 
        public override void DoSomething()
        {
            var x = 0;
            x++;
            "somestring".ToString();
            base.DoSomething();
        }

        public Task DoSomethingAsync()
        {
            return Task.Run(() => DoSomething());
        }

        public class NestedClass { }
    }

    //public abstract class SomeBase
    //{  
    //    public abstract Task<int> GetValueAsync();
    //}
}
