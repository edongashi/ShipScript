using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.ClearScript;

namespace ShipScript.RShipCore.Compilers
{
    public class DllCompiler : IModuleCompiler
    {
        #region Static lookup

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
                var requestingAssembly = args.RequestingAssembly;
                var requestedAssembly = new AssemblyName(args.Name);
                var targetName = requestedAssembly.Name + ".dll";
                FileInfo[] dlls;
                string dll;
                string directory;
                if (requestingAssembly == null || !LookupPaths.TryGetValue(args.RequestingAssembly, out directory))
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

        #endregion

        public void Compile(Module module)
        {
            var path = module.VirtualPath;
            var file = path.ResolvePath();
            var asm = Assembly.LoadFrom(file);
            var dir = path.ResolveDirectory();
            LookupPaths[asm] = dir;
            module.Exports = new HostTypeCollection(asm);
            module.Loaded = true;
        }
    }
}
