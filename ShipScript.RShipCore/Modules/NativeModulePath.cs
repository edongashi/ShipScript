using System;
using System.IO;
using System.Reflection;
using ShipScript.RShipCore.Helpers;

namespace ShipScript.RShipCore
{
    public class NativeModulePath : IVirtualPath
    {
        public NativeModulePath(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }

        public string ResolvePath()
        {
            return PathHelpers.GetAssemblyPath();
        }

        public string ResolveDirectory()
        {
            return PathHelpers.GetAssemblyDirectory();
        }

        public string ResolveExtension()
        {
            return "[native]";
        }

        public string ResolveContent()
        {
            throw new NotSupportedException();
        }
    }
}
