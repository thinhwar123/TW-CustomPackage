using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW.ACacheEverything
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ACacheMethod : Attribute
    {
        public ACacheMethod()
        {

        }
    }
}
