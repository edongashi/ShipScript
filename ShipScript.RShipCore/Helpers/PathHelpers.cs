using System;
using System.IO;
using System.Reflection;

namespace ShipScript.RShipCore.Helpers
{
    public static class PathHelpers
    {
        public static string GetAssemblyPath()
        {
            return (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
        }

        public static string GetAssemblyDirectory()
        {
            return Path.GetDirectoryName((new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath);
        }
    }
}
