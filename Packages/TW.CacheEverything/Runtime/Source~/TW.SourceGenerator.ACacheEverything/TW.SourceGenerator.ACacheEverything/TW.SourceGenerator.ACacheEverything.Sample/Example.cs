using System;


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
            [ACacheMethod]
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
            public void TestMethod()
            {
               
            }
        }
    }

    
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ACacheMethod : Attribute
    {
        public Type CacheType { get; }

        public ACacheMethod()
        {
            CacheType = typeof(Action);
        }
        public ACacheMethod(Type type)
        {
            CacheType = type;
        }
    }
}