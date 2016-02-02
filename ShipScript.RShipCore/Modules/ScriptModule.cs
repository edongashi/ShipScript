using System;
using System.Collections.Generic;

namespace ShipScript.RShipCore
{
    public class ScriptModule : Module
    {
        public ScriptModule(string id, IModuleLoader loader)
            : base(id, new ScriptModulePath(id), null, loader)
        {
        }

        public sealed override object Exports
        {
            get
            {
                if (Loaded)
                {
                    return base.Exports;
                }

                Loader.Compilers[".js"].Compile(this);
                if (!Loaded)
                {
                    throw new InvalidOperationException("Invalid state reached.");
                }

                return base.Exports;
            }
            set
            {
                base.Exports = value;
            }
        }

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>
        {
            [nameof(Exports)] = "exports"
        };
    }
}
