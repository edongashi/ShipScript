using System.Collections.Generic;

namespace ShipScript.RShipCore.AbstractStdOut
{
    /// <summary>
    /// Serves as a module that proxies calls to an actual output.
    /// </summary>
    public class StdOut : IStdOutWriter
    {
        public StdOut(IStdOutWriter writer)
        {
            Writer = writer;
        }

        public IStdOutWriter Writer { get; set; }

        public ColorScheme Colors { get; } = ColorScheme.Instance;

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
            [nameof(Colors)] = "colors",
            [nameof(Write)] = "write",
            [nameof(WriteLine)] = "writeln"
        };
    }
}
