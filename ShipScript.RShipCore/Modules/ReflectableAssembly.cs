using System;
using System.Collections.Generic;
using System.Reflection;

namespace ShipScript.RShipCore
{
    public class ReflectableAssembly : IReflectable
    {
        public ReflectableAssembly(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; }

        public object CreateObject(string name, params object[] args)
        {
            var type = Assembly.GetType(name);
            return Activator.CreateInstance(type, args);
        }
    }
}
