using System.Collections.Generic;
using Microsoft.ClearScript.V8;

namespace ShipScript.RShipCore
{
    public class ModuleLoaderFactory : IModuleLoaderFactory
    {
        public IModuleLoader Create(V8ScriptEngine evaluator, Dictionary<string, Module> nativeModules,
            Dictionary<string, IModuleCompiler> compilers,
            IModulePathResolver pathResolver)
        {
            return new ModuleLoader(evaluator, nativeModules, compilers, pathResolver);
        }
    }
}
