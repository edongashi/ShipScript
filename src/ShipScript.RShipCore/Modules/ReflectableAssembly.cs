using System;
using System.Linq;
using System.Reflection;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace ShipScript.RShipCore
{
    [Obsolete("Modules return HostTypeCollection by default")]
    public class ReflectableAssembly : IReflectable
    {
        private readonly V8ScriptEngine evaluator;

        public ReflectableAssembly(Assembly assembly, V8ScriptEngine evaluator)
        {
            this.evaluator = evaluator;
            Assembly = assembly;
        }
        
        public Assembly Assembly { get; }
        
        [ScriptMember("getAssembly")]
        public object GetAssembly()
        {
            return new HostTypeCollection(Assembly);
        }

        [ScriptMember("create")]
        public object CreateObject(string name, params object[] args)
        {
            var type = Assembly.GetType(name);
            return Activator.CreateInstance(type, args);
        }

        [ScriptMember("getType")]
        public object LoadType(string name, params object[] args)
        {
            var type = Assembly.GetType(name);
            return new ExtendedHostFunctions().type(type);
        }

        [ScriptMember("getEnum")]
        public object GetEnum(string name)
        {
            var type = Assembly.GetType(name);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException("Requested type is not an enum.");
            }

            return new ExtendedHostFunctions().type(type);
        }

        [ScriptMember("getEnumNumeric")]
        [NativeObject("Object")]
        public object GetEnumNumeric(string name)
        {
            var type = Assembly.GetType(name);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException("Requested type is not an enum.");
            }

            var names = Enum.GetNames(type).ToList();
            var count = names.Count - 1;
            var json = "JSON.parse('{";
            string key;
            int val;
            for (int i = 0; i < count; i++)
            {
                key = names[i];
                val = (int)Enum.Parse(type, key);
                json += $"\"{key}\":{val},";
            }

            key = names[count];
            val = (int)Enum.Parse(type, key);
            json += $"\"{key}\":{val}}}')";
            return evaluator.Evaluate(json);
        }
    }
}
