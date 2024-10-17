using System;
using System.Linq;
using TestExample.NewNamespace;


namespace TestExample
{
    namespace NewNamespace
    {
         public partial class Example
        {
            public partial class Example2
            {
                [ACacheMethod]
                public TestClass1 TestMethod2(int a)
                {
                    Console.WriteLine("TestMethod");
                    return new TestClass1();
                }
            }
            [ACacheMethod()]
            public void TestMethod()
            {
                Console.WriteLine("TestMethod");
            }

            public void TestMethod2()
            {
                // TestMethodCache?.Invoke();
            }
        }
        
    }

    public partial class TestClass1
    {
        public partial class TestClass2
        {
            [ACacheMethod("TestExample.NewNamespace")]
            public void TestMethod(Example example)
            {
               
            }
        }
    }

    
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ACacheMethod : Attribute
    {
        private string[] Libraries { get; set; }
        public ACacheMethod()
        {
            Libraries = new[] {"System"};
        }
        public ACacheMethod(params string[] libraries)
        {
            if (!libraries.Contains("System"))
            {
                libraries = libraries.Append("System").Distinct().ToArray();
            }
            Libraries = libraries ;
        }
    }
}