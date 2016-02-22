using System.IO;

namespace ShipScript.RShipCore
{
    public class FilePath : IVirtualPath
    {
        public FilePath(string path)
        {
            Identifier = path;
        }

        public string Identifier { get; }

        public string ResolvePath()
        {
            return Identifier;
        }

        public string ResolveDirectory()
        {
            return Path.GetDirectoryName(Identifier);
        }

        public string ResolveExtension()
        {
            return Path.GetExtension(Identifier);
        }

        public string ResolveContent()
        {
            return File.ReadAllText(Identifier);
        }
    }
}
