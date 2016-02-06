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
            engine.DefaultAccess = ScriptAccess.None;
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

            engine.Script.EngineInternal.isVoid = new Func<object, bool>(obj => obj is VoidResult);
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

        public bool ExecuteAsCommand { get; set; }

        public bool Sleeping { get; private set; }

        public bool FullAccess
        {
            get
            {
                return engine.DefaultAccess != ScriptAccess.None;
            }
            set
            {
                engine.DefaultAccess = value ? ScriptAccess.Full : ScriptAccess.None;
            }
        }

        public object Require(string request)
        {
            return coreModule.Require(request).Exports;
        }

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
                Console.WriteErr(ex.GetScriptStack());
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

        public void ExecuteCommand(string command)
        {
            Console.WriteCommand(command);
            try
            {
                object result;
                if (ExecuteAsCommand)
                {
                    result = engine.ExecuteCommand(command);
                }
                else
                {
                    #warning Concurrency issue
                    engine.Script.EngineInternal.command = command;
                    result = engine.Evaluate("Command", "eval(EngineInternal.command)");
                }

                Console.WriteResult(result);
            }
            catch (Exception ex)
            {
                Console.WriteErr(ex.GetScriptStack());
            }
        }

        public bool ScriptEvaluate()
        {
            try
            {
                engine.Execute("eval", @"EngineInternal.evalResult = eval(EngineInternal.evalCode)");
                return true;
            }
            catch (Exception ex)
            {
                engine.Script.EngineInternal.evalError = ex.GetScriptStack();
                return false;
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
    }
}
