using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShipScript.RShipCore.Compilers
{
    public class DllCompiler : IModuleCompiler
    {
        public static Dictionary<Assembly, string> LookupPaths { get; }

        static DllCompiler()
        {
            LookupPaths = new Dictionary<Assembly, string>();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var reqiestomgAssembly = args.RequestingAssembly;
                var requestedAssembly = new AssemblyName(args.Name);
                var targetName = requestedAssembly.Name + ".dll";
                FileInfo[] dlls;
                string dll;
                string directory;
                if (reqiestomgAssembly == null || !LookupPaths.TryGetValue(args.RequestingAssembly, out directory))
                {
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var dir in LookupPaths.Values)
                    {
                        dlls = new DirectoryInfo(dir).GetFiles("*.dll");
                        dll = dlls.FirstOrDefault(f => string
                            .Equals(f.Name, targetName, StringComparison.OrdinalIgnoreCase))
                            ?.FullName;
                        if (dll != null)
                        {
                            return Assembly.LoadFrom(dll);
                        }
                    }

                    return null;
                }

                dlls = new DirectoryInfo(directory).GetFiles("*.dll");
                dll = dlls.FirstOrDefault(f => string
                    .Equals(f.Name, targetName, StringComparison.OrdinalIgnoreCase))
                    ?.FullName;
                return dll == null ? null : Assembly.LoadFrom(dll);
            }
            catch
            {
                return null;
            }
        }

        public void Compile(Module module)
        {
            var path = module.VirtualPath;
            var file = path.ResolvePath();
            var asm = Assembly.LoadFrom(file);
            var dir = path.ResolveDirectory();
            LookupPaths[asm] = dir;
            var type = asm.GetType("ShipScript.Loader", false);
            if (type != null)
            {
                var method = type.GetMethod("Load", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null);
                if (method.ReturnType == typeof(void))
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
