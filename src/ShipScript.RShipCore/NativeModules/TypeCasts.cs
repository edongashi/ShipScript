using Microsoft.ClearScript;

namespace ShipScript.RShipCore
{
    [ModuleExports]
    public class TypeCasts
    {
        [ScriptMember("to")]
        public T Custom<T>(object value) => (T)value;

        [ScriptMember("int")]
        public int ToInt(object value) => (int)value;

        [ScriptMember("self")]
        public object Unwrap(object value) => value;
    }
}
