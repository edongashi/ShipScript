using System;
using System.Collections.Generic;
using ShipScript.Common;

namespace ShipScript.RShipCore
{
    public class ModuleLoader : IModuleLoader
    {
        private Module mainModule;

        public ModuleLoader(IScriptEvaluator evaluator,
            Dictionary<string, Module> nativeModules,
            Dictionary<string, IModuleCompiler> compilers,
            IModulePathResolver pathResolver)
        {
            Evaluator = evaluator;
            NativeModules = nativeModules;
            LoadedModules = new Dictionary<string, Module>(StringComparer.OrdinalIgnoreCase);
            Compilers = compilers;
            PathResolver = pathResolver;
        }

        public IScriptEvaluator Evaluator { get; }

        public Dictionary<string, Module> NativeModules { get; }

        public Dictionary<string, Module> LoadedModules { get; }

        public Dictionary<string, IModuleCompiler> Compilers { get; }

        public IModulePathResolver PathResolver { get; set; }

        public Module MainModule
        {
            get { return mainModule; }
            set
            {
                if (value?.Loader == this)
                {
                    mainModule = value;
                }
            }
        }

        public Module Load(string request, Module parent, bool isMain)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (parent != null && parent.Loader != this)
            {
                throw new InvalidOperationException("Parent module is initialized from a different loader.");
            }

            Module module;
            if (NativeModules.TryGetValue(request, out module))
            {
                return module;
            }

            var virtualPath = PathResolver.Resolve(request, parent);
            var identifier = virtualPath?.Identifier;

            if (identifier == null)
            {
                throw new InvalidOperationException("Module not found.");
            }

            if (LoadedModules.TryGetValue(identifier, out module))
            {
                return module;
            }

            module = new Module(identifier, virtualPath, parent, this);
            var oldMain = MainModule;
            if (isMain)
            {
                MainModule = module;
            }

            var extension = virtualPath.ResolveExtension();

            IModuleCompiler compiler;
            if (!Compilers.TryGetValue(extension, out compiler))
            {
                throw new InvalidOperationException("Invalid file extension.");
            }

            LoadedModules[identifier] = module;
            try
            {
                compiler.Compile(module);
            }
            catch
            {
                LoadedModules.Remove(identifier);
                if (isMain)
                {
                    MainModule = oldMain;
                }

                throw;
            }

            return module;
        }
    }
}
