using System.Collections.Generic;
using ShipScript.Common;

namespace ShipScript.RShipCore
{
    [NativeObject("Function")]
    public class RequireFunction : IScriptNativeObject
    {
        private readonly IScriptEvaluator scriptEvaluator;

        private object function;
        private bool loaded;

        public RequireFunction(IScriptEvaluator scriptEvaluator, IModuleLoader loader, Module module)
        {
            this.scriptEvaluator = scriptEvaluator;
            Loader = loader;
            Module = module;
        }

        public IModuleLoader Loader { get; }

        public Module Module { get; }

        public Module Main => Loader.MainModule;

        public Module Invoke(string id)
        {
            return Module.Require(id);
        }

        public string Resolve()
        {
            return Module.FileName;
        }

        public object GetScriptObject()
        {
            if (loaded)
            {
                return function;
            }

            function = ((dynamic)scriptEvaluator.Evaluate("native", @"
                (function (nativeRequire) { 
                    function require(id) {
                        var module = nativeRequire.invoke(id);
                        return module.exports;
                    }
                    var requireToString = () => 'function require() { [native code] }';
                    Object.defineProperty(require, 'toString', { value: requireToString });
                    Object.defineProperty(requireToString, 'toString', { value: toString });

                    function resolve() { return nativeRequire.resolve(); }
                    var resolveToString = () => 'function resolve() { [native code] }';
                    Object.defineProperty(resolve, 'toString', { value: resolveToString });
                    Object.defineProperty(resolveToString, 'toString', { value: toString });

                    Object.defineProperty(require, 'resolve', { value: resolve, enumerable: true  });
                    Object.defineProperty(require, 'main', { get: () => nativeRequire.main });
                    return require;
                }).valueOf()"))(this);
            loaded = true;
            return function;
        }

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>
        {
            {"Invoke", "invoke"},
            {nameof(Resolve), "resolve"}
        };
    }
}
