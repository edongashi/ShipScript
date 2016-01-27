namespace ShipScript.RShipCore.Compilers
{
    public class ScriptCompiler : IModuleCompiler
    {
        public void Compile(Module module)
        {
            var path = module.VirtualPath;
            var evaluator = module.Evaluator;
            var code = path.ResolveContent();
            var wrapper = (dynamic)evaluator.Evaluate($@"
                (function (exports, require, module, __filename, __dirname) {{
                    {code}
                }}).valueOf()");

            var exports = evaluator.Evaluate("new Object()");
            module.Exports = exports;
            var fileName = path.ResolvePath();
            var dirName = path.ResolveDirectory();
            var require = module.NativeRequire;
            wrapper(exports, require, module, fileName, dirName);
            module.Loaded = true;
        }
    }
}
