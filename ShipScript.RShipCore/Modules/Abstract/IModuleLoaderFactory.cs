using System.Collections.Generic;
using ShipScript.Common;

namespace ShipScript.RShipCore
{
    public interface IModuleLoaderFactory
    {
        IModuleLoader Create(
            IScriptEvaluator evaluator,
            Dictionary<string, Module> nativeModules,
            Dictionary<string, IModuleCompiler> compilers,
            IModulePathResolver pathResolver);
    }
}
