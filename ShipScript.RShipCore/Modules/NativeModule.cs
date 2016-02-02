using System.Collections.Generic;

namespace ShipScript.RShipCore
{
    public class NativeModule : Module
    {
        public NativeModule(string id, IModuleLoader loader, object exports)
            : base(id, new NativeModulePath(id), null, loader)
        {
            OverrideExports(exports);
            Loaded = true;
        }

        public override sealed object Exports
        {
            get
            {
                return base.Exports;
            }

            set
            {
                // ignored
            }
        }

        public void OverrideExports(object newValue)
        {
            base.Exports = newValue;
        }

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>
        {
            [nameof(Exports)] = "exports"
        };
    }
}
