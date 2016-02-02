using System.Collections.Generic;
using ShipScript.Common;
using ShipScript.RShipCore.NativeTypes;

namespace ShipScript.RShipCore
{
    public class Module
    {
        public Module(string id, IVirtualPath virtualPath, Module parent, IModuleLoader loader)
        {
            Evaluator = loader.Evaluator;
            Id = id;
            Parent = parent;
            Loader = loader;
            VirtualPath = virtualPath;
            parent?.Children?.Add(this);
            Loaded = false;
            Children = new List<Module>();
            RequireFunction = new RequireFunction(Evaluator, loader, this);
        }

        public IScriptEvaluator Evaluator { get; }

        public string Id { get; }

        public string FileName => VirtualPath.ResolvePath();

        public IVirtualPath VirtualPath { get; }

        public Module Parent { get; }

        public IModuleLoader Loader { get; }

        public List<Module> Children { get; }

        public bool Loaded { get; set; }

        public virtual object Exports { get; set; }

        public RequireFunction RequireFunction { get; }

        [NativeObject("Function")]
        public object NativeRequire => RequireFunction.GetScriptObject();

        [NativeObject("Array")]
        public object NativeChildren => new JArray(Evaluator, Children).GetScriptObject();

        public Module Require(string id)
        {
            return Loader.Load(id, this);
        }

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>
        {
            [nameof(Exports)] = "exports"
        };
    }
}
