using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpbot
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Version : Attribute
    {
        public string Ver { get; private set; }

        public Version(string version)
        {
            Ver = version;
        }
    }
}
