using System.Collections.Generic;
using ShipScript.Common;

namespace ShipScript.RShipCore
{
    public class ModuleLoaderFactory : IModuleLoaderFactory
    {
        public IModuleLoader Create(IScriptEvaluator evaluator, Dictionary<string, Module> nativeModules, Dictionary<string, IModuleCompiler> compilers,
            IModulePathResolver pathResolver)
        {
            return new ModuleLoader(evaluator, nativeModules, compilers, pathResolver);
        }
    }
}
