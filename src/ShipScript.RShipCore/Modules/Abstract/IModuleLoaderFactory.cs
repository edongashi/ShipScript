using System.Collections.Generic;
using Microsoft.ClearScript.V8;

namespace ShipScript.RShipCore
{
    public interface IModuleLoaderFactory
    {
        IModuleLoader Create(
            V8ScriptEngine evaluator,
            Dictionary<string, Module> nativeModules,
            Dictionary<string, IModuleCompiler> compilers,
            IModulePathResolver pathResolver);
    }
}
