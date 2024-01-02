using System;

namespace TW.AEnumQuickConvert
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class AEnumConversion : Attribute
    {
        public string Name { get; set; }

        public AEnumConversion()
        {
            Name = "";
        }

        public AEnumConversion(string name)
        {
            Name = name;
        }
    }
}
