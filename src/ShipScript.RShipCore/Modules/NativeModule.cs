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

        public sealed override object Exports
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
    }
}
