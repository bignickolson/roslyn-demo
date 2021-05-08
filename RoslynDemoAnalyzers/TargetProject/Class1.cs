using System;
using TestNamespace.Enums;

namespace TargetProject
{
    public class Class1
    {
        public Class1()
        {
            var status = PersonStatus.Active;
            Console.WriteLine(status);
        }
    }
}
