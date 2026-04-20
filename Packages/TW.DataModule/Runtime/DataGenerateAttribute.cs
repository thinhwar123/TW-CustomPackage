using System;

namespace DataModule
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct,AllowMultiple = false, Inherited = true)]
    public class DataGenerateAttribute : Attribute
    {
        public int Order { get; set; } = 0;

        public DataGenerateAttribute(int order = 0)
        {
            Order = order;
        }
    }
}
