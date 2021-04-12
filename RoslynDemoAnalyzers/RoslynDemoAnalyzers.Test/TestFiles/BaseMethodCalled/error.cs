using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetConsoleApp.Models
{
    public class ChildModel : BaseModel
    {
        public override void DoSomething()
        {
            "didn't call my important method".ToString();
        }
    }


    public class BaseModel
    {
        public virtual void DoSomething()
        {
            // i do some important stuff
        }
    }
}
