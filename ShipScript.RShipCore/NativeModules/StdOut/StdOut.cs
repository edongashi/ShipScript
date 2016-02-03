using System.Collections.Generic;

namespace ShipScript.RShipCore.StdOut
{
    /// <summary>
    /// Serves as a module that proxies calls to an actual output.
    /// </summary>
    [ModuleExports]
    public class StdOut : IStdOutWriter
    {
        public StdOut()
        {
            
        }

        public StdOut(IStdOutWriter writer)
        {
            Writer = writer;
        }

        public IStdOutWriter Writer { get; set; }

        public StdOutColors Colors { get; } = StdOutColors.Instance;

        public void Write(string value)
        {
            Writer?.Write(value);
        }

        public void Write(string value, int color)
        {
            Writer?.Write(value, color);
        }

        public void WriteLine()
        {
            Writer?.WriteLine();
        }

        public void WriteLine(string value)
        {
            Writer?.WriteLine(value);
        }

        public void WriteLine(string value, int color)
        {
            Writer?.WriteLine(value, color);
        }

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>()
        {
            [nameof(Colors)] = "color",
            [nameof(Write)] = "write",
            [nameof(WriteLine)] = "writeln"
        };
    }
}
