using System;
using System.Collections.Generic;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using ShipScript.RShipCore.Compilers;
using ShipScript.RShipCore.Extensions;
using ShipScript.RShipCore.Timers;

[assembly: NoDefaultScriptAccess]

namespace ShipScript.RShipCore
{
    public partial class RShipCore
    {
        private readonly V8ScriptEngine engine;
        private readonly IModuleLoader loader;
        private readonly IModulePathResolver pathResolver;
        private readonly NativeModule coreModule;

        public RShipCore(IModulePathResolver pathResolver, IModuleLoaderFactory loaderFactory)
        {
            engine = new V8ScriptEngine(V8ScriptEngineFlags.DisableGlobalMembers)
            {
                DefaultAccess = ScriptAccess.Full,
                AllowReflection = true
            };

            Engine = engine;
            this.pathResolver = pathResolver;
            NativeModules = new Dictionary<string, Module>();
            Compilers = new Dictionary<string, IModuleCompiler>();
            loader = loaderFactory.Create(Engine, NativeModules, Compilers, pathResolver);

            Console = new VirtualConsole.Console(null, Engine);
            StdOut = new StdOut.StdOut();

            var scriptCompiler = new ScriptCompiler();
            Compilers[".ship"] = scriptCompiler;
            Compilers[".js"] = scriptCompiler;
            Compilers[".json"] = new JsonCompiler();
            Compilers[".dll"] = new DllCompiler();
            coreModule = new NativeModule("core", loader, this);

            NativeModules["core"] = coreModule;
            NativeModules["console"] = new NativeModule("console", loader, Console);
            NativeModules["stdout"] = new NativeModule("stdout", loader, StdOut);
            NativeModules["cast"] = new NativeModule("cast", loader, new TypeCasts());
            NativeModules["timer"] = new NativeModule("timer", loader, new TimerController(Console.ErrStream));
            NativeModules["host"] = new NativeModule("host", loader, new HostFunctions());
            NativeModules["xhost"] = new NativeModule("xhost", loader, new ExtendedHostFunctions());
            NativeModules["async"] = new NativeModule("async", loader, new Async(engine));

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

        public V8ScriptEngine Engine { get; }

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

        #region Native Modules

        [ModuleExports]
        public VirtualConsole.Console Console { get; }

        [ModuleExports]
        public StdOut.StdOut StdOut { get; }

        #endregion

        [ScriptMember("sleeping")]
        public bool Sleeping { get; private set; }

        [ScriptMember("replOnSleep")]
        public bool ReplOnSleep { get; private set; }

        [ScriptMember("all")]
        public bool FullAccess
        {
            get { return engine.DefaultAccess != ScriptAccess.None; }
            set { engine.DefaultAccess = value ? ScriptAccess.Full : ScriptAccess.None; }
        }

        [NativeObjectHint]
        public object Require(string request)
        {
            return coreModule.RequireFunction.Invoke(request);
        }

        [ScriptMember("addNativeModule")]
        public void AddNativeModule(string name, object exports)
        {
            if (NativeModules.ContainsKey(name))
            {
                throw new InvalidOperationException("A native module with the same name already exists.");
            }

            NativeModules[name] = new NativeModule(name, loader, exports);
        }

        [ScriptMember("exposeRequire")]
        public object ExposeGlobalRequire()
        {
            return engine.Script.require = coreModule.RequireFunction.GetScriptObject();
        }

        public void Execute(string code) => ExecuteWrapped(code);

        [ScriptMember("executeCommand")]
        public string ExecuteCommand(string command) => Engine.ExecuteCommand(command);

        private void ExecuteWrapped(string code)
        {
            engine.Execute($@"
                (function () {{
                    {code}
                }})()");
        }
    }
}
