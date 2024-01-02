using System;

namespace TW.SourcesGenerator.AEnumQuickConvert.Example
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
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class AEnumIgnore : Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class AEnumQuickConvert : Attribute
    {

    }
}