using System;

namespace TW.ACacheEverything
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ASingletonBindingAttribute : Attribute
    {
        public Type Type { get; }

        public ASingletonBindingAttribute(Type type)
        {
            Type = type;
        }
    }
}