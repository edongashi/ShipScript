using ShipScript.Common;

namespace ShipScript.RShipCore.StdOut
{
    public interface IStdOutWriter
    {
        [ScriptMember("write")]
        void Write(string value);

        [ScriptMember("write")]
        void Write(string value, int color);

        [ScriptMember("writeln")]
        void WriteLine();

        [ScriptMember("writeln")]
        void WriteLine(string value);

        [ScriptMember("writeln")]
        void WriteLine(string value, int color);
    }
}
