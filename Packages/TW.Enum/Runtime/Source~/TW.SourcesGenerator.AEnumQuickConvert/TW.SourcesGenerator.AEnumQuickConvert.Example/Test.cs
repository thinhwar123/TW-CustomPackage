using TW.SourcesGenerator.AEnumQuickConvert.Example;
using TW.SourcesGenerator.AEnumQuickConvert.Example.MainNamespace;
using TW.SourcesGenerator.AEnumQuickConvert.Example.MainNamespace.TestNamespace;

namespace TW.SourcesGenerator.AEnumQuickConvert.Example
{
    namespace MainNamespace
    {
         namespace TestNamespace
        {
            [AEnumQuickConvert]

            public enum MyEnum
            {
                [AEnumConversion("Add")] A,
                B,
                [AEnumIgnore] C,
            }
        }
    }
}

namespace TW.SourcesGenerator.AEnumQuickConvert.Example
{
    namespace MainNamespace
    {
        public enum BigEnum
        {
            [AEnumConversion("Add")] A,
            B,
            [AEnumIgnore] C,
        }
    }
}

namespace TW.SourcesGenerator.AEnumQuickConvert.Example
{
    public class Test
    { 
        public void TestMethod()
        {
            var myEnum = MyEnum.A;
            myEnum.ToString();

        }
    }
}