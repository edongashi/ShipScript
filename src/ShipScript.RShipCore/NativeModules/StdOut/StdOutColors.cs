using Microsoft.ClearScript;

namespace ShipScript.RShipCore.StdOut
{
    public class StdOutColors
    {
        public static readonly StdOutColors Instance = new StdOutColors();

        [ScriptMember("black")]
        public int Black => 0;

        [ScriptMember("dblue")]
        public int DarkBlue => 1;

        [ScriptMember("dgreen")]
        public int DarkGreen => 2;

        [ScriptMember("dcyan")]
        public int DarkCyan => 3;

        [ScriptMember("dred")]
        public int DarkRed => 4;

        [ScriptMember("dmagenta")]
        public int DarkMagenta => 5;

        [ScriptMember("dyellow")]
        public int DarkYellow => 6;

        [ScriptMember("gray")]
        public int Gray => 7;

        [ScriptMember("dgray")]
        public int DarkGray => 8;

        [ScriptMember("blue")]
        public int Blue => 9;

        [ScriptMember("green")]
        public int Green => 10;

        [ScriptMember("cyan")]
        public int Cyan => 11;

        [ScriptMember("red")]
        public int Red => 12;

        [ScriptMember("magenta")]
        public int Magenta => 13;

        [ScriptMember("yellow")]
        public int Yellow => 14;

        [ScriptMember("white")]
        public int White => 15;
    }
}
