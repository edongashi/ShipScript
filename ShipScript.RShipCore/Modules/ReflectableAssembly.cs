using System;
using System.Reflection;
using ShipScript.Common;

namespace ShipScript.RShipCore
{
    public class ReflectableAssembly : IReflectable
    {
        public ReflectableAssembly(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; }

        [ScriptMember("create")]
        public object CreateObject(string name, params object[] args)
        {
            var type = Assembly.GetType(name);
            return Activator.CreateInstance(type, args);
        }
    }
}
