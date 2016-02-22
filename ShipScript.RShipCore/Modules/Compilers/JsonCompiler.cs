namespace ShipScript.RShipCore.Compilers
{
    public class JsonCompiler : IModuleCompiler
    {
        public void Compile(Module module)
        {
            var evaluator = module.Evaluator;
            var path = module.VirtualPath;
            var content = path.ResolveContent();
            module.Exports = ((dynamic)evaluator.Evaluate("JSON.parse"))(content);
            module.Loaded = true;
        }
    }
}
