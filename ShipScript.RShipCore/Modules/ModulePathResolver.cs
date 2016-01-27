using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShipScript.RShipCore
{
    public class ModulePathResolver : IModulePathResolver
    {
        public ModulePathResolver(string modulesPath, IEnumerable<string> extensions, string defaultFileName)
        {
            ModulesPath = modulesPath;
            Extensions = extensions;
            DefaultFileName = defaultFileName;
        }

        public string ModulesPath { get; set; }

        public IEnumerable<string> Extensions { get; set; }

        public string DefaultFileName { get; set; }

        public IVirtualPath Resolve(string path, Module parent)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid file path.");
            }

            string combinedPath;
            if (path.StartsWith("/"))
            {
                combinedPath = path.Substring(1);
            }
            else if (path.Length > 1)
            {
                var start = path.Substring(0, 2);
                if (start == "./" || start == "../")
                {
                    var parentDirectory = Path.GetDirectoryName(parent.FileName);
                    if (parentDirectory == null)
                    {
                        return null;
                    }

                    combinedPath = Path.Combine(parentDirectory, path);
                }
                else
                {
                    combinedPath = Path.Combine(ModulesPath, path);
                }
            }
            else
            {
                combinedPath = Path.Combine(ModulesPath, path);
            }

            var result = SearchFile(combinedPath) ?? SearchDirectory(combinedPath);
            if (result == null)
            {
                return null;
            }

            return new FilePath(result);
        }

        private string SearchFile(string file)
        {
            if (Path.HasExtension(file))
            {
                return File.Exists(file) ? Path.GetFullPath(file) : null;
            }

            return Extensions.Any(extension => File.Exists(file + extension)) 
                ? Path.GetFullPath(file) 
                : null;
        }

        private string SearchDirectory(string directory)
        {
            if (!Directory.Exists(directory)) return null;
            return (from extension in Extensions
                    let file = Path.Combine(directory, DefaultFileName)
                    where File.Exists(file + extension)
                    select Path.GetFullPath(file))
                    .FirstOrDefault();
        }
    }
}
