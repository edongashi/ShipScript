using System;
using System.Reflection;

namespace ShipScript.RShipCore.Compilers
{
    public class DllCompiler : IModuleCompiler
    {
        public void Compile(Module module)
        {
            var path = module.VirtualPath;
            var file = path.ResolvePath();
            var asm = Assembly.LoadFrom(file);
            // TODO: Handle assembly dependencies
            var type = asm.GetType("ShipScript.Loader", false);
            if (type != null)
            {
                var method = type.GetMethod("Load", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null);
                if (method.ReturnType == typeof (void))
                {
                    method.Invoke(null, null);
                    module.Exports = new ReflectableAssembly(asm);
                }
                else
                {
                    module.Exports = method.Invoke(null, null);
                }
            }
            else
            {
                module.Exports = new ReflectableAssembly(asm);
            }

            module.Loaded = true;
        }
    }
}
