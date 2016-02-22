using ShipScript.Common;

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

        [ScriptMember("color")]
        public StdOutColors Colors { get; } = StdOutColors.Instance;

        [ScriptMember("write")]
        public void Write(string value)
        {
            Writer?.Write(value);
        }

        [ScriptMember("write")]
        public void Write(string value, int color)
        {
            Writer?.Write(value, color);
        }

        [ScriptMember("writeln")]
        public void WriteLine()
        {
            Writer?.WriteLine();
        }

        [ScriptMember("writeln")]
        public void WriteLine(string value)
        {
            Writer?.WriteLine(value);
        }

        [ScriptMember("writeln")]
        public void WriteLine(string value, int color)
        {
            Writer?.WriteLine(value, color);
        }
    }
}
