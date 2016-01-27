using System.Collections.Generic;
using ShipScript.Common;

namespace ShipScript.RShipCore
{
    public interface IModuleLoader
    {
        IScriptEvaluator Evaluator { get; }

        Dictionary<string, Module> NativeModules { get; }

        Dictionary<string, Module> LoadedModules { get; }

        Dictionary<string, IModuleCompiler> Compilers { get; }

        Module MainModule { get; set; }

        Module Load(string request, Module parent, bool isMain = false);
    }

}
