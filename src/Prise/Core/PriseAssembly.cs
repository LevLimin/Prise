using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Prise.Utils;

namespace Prise
{
    public class PriseAssembly : IAssemblyShim
    {
        public PriseAssembly(Assembly assembly)
        {
            this.Assembly = assembly.ThrowIfNull(nameof(assembly));
        }

        public Assembly Assembly { get; private set; }

        public IEnumerable<Type> Types
        {
            get
            {
                if (this.Assembly == null)
                    return Enumerable.Empty<Type>();

                return this.Assembly.GetTypes();
            }
        }
    }
}