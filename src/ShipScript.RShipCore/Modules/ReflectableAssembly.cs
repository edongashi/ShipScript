using System;
using System.Linq;
using System.Reflection;
using ShipScript.Common;

namespace ShipScript.RShipCore
{
    public class ReflectableAssembly : IReflectable
    {
        private readonly IScriptEvaluator evaluator;

        public ReflectableAssembly(Assembly assembly, IScriptEvaluator evaluator)
        {
            this.evaluator = evaluator;
            Assembly = assembly;
        }
        
        [ScriptMember("assembly", ScriptAccess.ReadOnly)]
        public Assembly Assembly { get; }

        [ScriptMember("create")]
        public object CreateObject(string name, params object[] args)
        {
            var type = Assembly.GetType(name);
            return Activator.CreateInstance(type, args);
        }

        [ScriptMember("type")]
        public object LoadType(string name, params object[] args)
        {
            var type = Assembly.GetType(name);
            return type;
        }

        [ScriptMember("enum")]
        [NativeObject("Object")]
        public object GetEnum(string name)
        {
            var type = Assembly.GetType(name);
            if (type.BaseType == typeof(Enum))
            {
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

            throw new InvalidOperationException("Requested type is not an enum.");
        }
    }
}
