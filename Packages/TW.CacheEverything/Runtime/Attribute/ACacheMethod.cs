using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW.ACacheEverything
{
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
            Libraries = libraries ;
        }
    }
}
