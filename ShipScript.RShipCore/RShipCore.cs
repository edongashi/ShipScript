using System;
using System.Collections.Generic;
using ShipScript.Common;
using ShipScript.RShipCore.Compilers;
using ShipScript.RShipCore.Extensions;
using ShipScript.RShipCore.Helpers;
using ShipScript.RShipCore.Pipes;

namespace ShipScript.RShipCore
{
    public class RShipCore : ICommandReceiver
    {
        public static string CoreName => "RShipCore";
        public static string CoreVersion => "1.0.0";

        private readonly IScriptEngine engine;
        private readonly IModuleLoader loader;
        private readonly IModulePathResolver pathResolver;
        private readonly NativeModule coreModule;

        public RShipCore(IScriptEngine engine, IModulePathResolver pathResolver, IModuleLoaderFactory loaderFactory)
        {
            this.engine = engine;
            Evaluator = engine;
            engine.DefaultScriptAccess = ScriptAccessEnum.None;
            this.pathResolver = pathResolver;
            NativeModules = new Dictionary<string, Module>();
            Compilers = new Dictionary<string, IModuleCompiler>();
            loader = loaderFactory.Create(Evaluator, NativeModules, Compilers, pathResolver);

            Console = new VirtualConsole.Console(null, Evaluator);
            StdOut = new StdOut.StdOut();

            CommandPipe = new CommandPipe(this);

            Compilers[".ship"] = Compilers[".js"] = new ScriptCompiler();
            Compilers[".json"] = new JsonCompiler();
            Compilers[".dll"] = new DllCompiler();
            coreModule = new NativeModule("core", loader, this);
            NativeModules["core"] = coreModule;
            NativeModules["console"] = new NativeModule("console", loader, Console);
            NativeModules["stdout"] = new NativeModule("stdout", loader, StdOut);
            NativeModules["host"] = new NativeModule("host", loader, engine.CreateHostFunctions());
            NativeModules["xhost"] = new NativeModule("xhost", loader, engine.CreateExtendedHostFunctions());
            foreach (var script in ScriptModules.Scripts.Keys)
            {
                NativeModules.Add(script, new ScriptModule(script, loader));
            }

            engine.Script.EngineInternal.isVoid = new Func<object, bool>(engine.IsVoidResult);
            ExecuteWrapped(@"
                Object.defineProperty(this, 'global', { value: this, enumerable: true });
                var engineInternal = this.EngineInternal;
                delete this.EngineInternal;
                Object.defineProperty(this, 'EngineInternal', { value: engineInternal });
            ");

            Sleeping = true;
        }

        public Dictionary<string, Module> NativeModules { get; }

        public Dictionary<string, IModuleCompiler> Compilers { get; }

        public IScriptEvaluator Evaluator { get; }

        [ModuleExports]
        public VirtualConsole.Console Console { get; }

        [ModuleExports]
        public StdOut.StdOut StdOut { get; }

        public CommandPipe CommandPipe { get; }

        public bool Sleeping { get; private set; }

        public Module Run(string request)
        {
            if (!Sleeping)
            {
                throw new InvalidOperationException("Running at this state is not permitted.");
            }

            Sleeping = false;
            try
            {
                return loader.Load(request, null, true);
            }
            catch (Exception ex)
            {
                PrintExceptionError(ex);
                Sleeping = true;
                throw;
            }
        }

        public void AddNativeModule(string name, object exports)
        {
            if (NativeModules.ContainsKey(name))
            {
                throw new InvalidOperationException("A native module with the same name already exists.");
            }

            NativeModules[name] = new NativeModule(name, loader, exports);
        }

        public object ExposeGlobalRequire()
        {
            return engine.Script.require = coreModule.RequireFunction.GetScriptObject();
        }

        public object ExposeExplore()
        {
            return engine.Script.explore = ((dynamic)engine.Evaluate(@"
                (function (nativeWrite) {
                    var explore = function (obj) {
                        for (var prop in obj) {
                            nativeWrite(prop);
                        }
                    }
                    var exploreToString = () => 'function explore() { [native code] }';
                    Object.defineProperty(explore, 'toString', { value: exploreToString });
                    Object.defineProperty(exploreToString, 'toString', { value: toString });
                    return explore;
                }).valueOf()"))(new Action<object>(Console.WriteCore));
        }

        public void EnableFullAccess()
        {
            engine.DefaultScriptAccess = ScriptAccessEnum.Full;
        }

        public void DisableFullAccess()
        {
            engine.DefaultScriptAccess = ScriptAccessEnum.None;
        }

        public void ExecuteCommand(string command)
        {
            Console.WriteCommand(command);
            try
            {
                var result = engine.ExecuteCommand(command);
                Console.WriteResult(result);
            }
            catch (Exception ex)
            {
                PrintExceptionError(ex);
            }
        }

        public void Sleep() => Sleeping = true;

        private void ExecuteWrapped(string code)
        {
            engine.Execute($@"
                (function () {{
                    {code}
                }})()");
        }

        private void PrintExceptionError(Exception ex)
        {
            var scriptException = ex.GetInnerMost<IScriptEngineException>();
            Console.WriteErr(scriptException != null
                ? StringHelpers.CleanupStackTrace(scriptException.ErrorDetails)
                : ex.Message);
        }

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>()
        {
            [nameof(CommandPipe)] = "!commandPipe",
            [nameof(AddNativeModule)] = "nativeModule",
            [nameof(EnableFullAccess)] = "enableFullAccess",
            [nameof(ExposeGlobalRequire)] = "exposeGlobalRequire",
            [nameof(Sleep)] = "sleep"
        };
    }
}
