using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace ShipScript.RShipCore
{
    [NativeObject("Function")]
    public class RequireFunction : IScriptNativeObject
    {
        private readonly V8ScriptEngine scriptEvaluator;

        private object function;
        private bool loaded;

        public RequireFunction(V8ScriptEngine scriptEvaluator, IModuleLoader loader, Module module)
        {
            this.scriptEvaluator = scriptEvaluator;
            Loader = loader;
            Module = module;
        }

        public IModuleLoader Loader { get; }

        public Module Module { get; }

        [ScriptMember("main")]
        public Module Main => Loader.MainModule;

        [ScriptMember("invoke")]
        [NativeObjectHint]
        public object Invoke(string id)
        {
            var module = Module.Require(id);
            var native = module.Exports as IScriptNativeObject;
            return native?.GetScriptObject() ?? module.Exports;
        }

        [ScriptMember("resolve")]
        public string Resolve(string id)
        {
            return Loader.Resolve(id, Module);
        }

        public object GetScriptObject()
        {
            if (loaded)
            {
                return function;
            }

            function = ((dynamic)scriptEvaluator.Evaluate("native", false, @"(
                function (nativeRequire) { 
                    function require(id) {
                        return nativeRequire.invoke(id);
                    }

                    function resolve(id) { return nativeRequire.resolve(id); }
                    Object.defineProperty(require, 'resolve', { value: resolve, enumerable: true  });
                    Object.defineProperty(require, 'main', { get: () => nativeRequire.main, enumerable: true });

                    var outer = toString;
                    function toString() { return 'function toString() { [native code] }' }
                    Object.defineProperty(toString, 'toString', { value: outer });

                    function requireToString() { return 'function require() { [native code] }'; }
                    Object.defineProperty(require, 'toString', { value: requireToString });
                    Object.defineProperty(requireToString, 'toString', { value: toString });
                    function resolveToString() { return 'function resolve() { [native code] }'; }
                    Object.defineProperty(resolve, 'toString', { value: resolveToString });
                    Object.defineProperty(resolveToString, 'toString', { value: toString });

                    return require;
                }).valueOf()"))(this);
            loaded = true;
            return function;
        }
    }
}
