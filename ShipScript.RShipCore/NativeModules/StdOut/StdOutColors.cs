using System.Collections.Generic;

namespace ShipScript.RShipCore.StdOut
{
    public class StdOutColors
    {
        public static readonly StdOutColors Instance = new StdOutColors();

        public int Black => 0;

        public int DarkBlue => 1;

        public int DarkGreen => 2;

        public int DarkCyan => 3;

        public int DarkRed => 4;

        public int DarkMagenta => 5;

        public int DarkYellow => 6;

        public int Gray => 7;

        public int DarkGray => 8;

        public int Blue => 9;

        public int Green => 10;

        public int Cyan => 11;

        public int Red => 12;

        public int Magenta => 13;

        public int Yellow => 14;

        public int White => 15;

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>
        {
            [nameof(Black)] = "black",
            [nameof(DarkBlue)] = "dblue",
            [nameof(DarkGreen)] = "dgreen",
            [nameof(DarkCyan)] = "dcyan",
            [nameof(DarkRed)] = "dred",
            [nameof(DarkMagenta)] = "dmagenta",
            [nameof(DarkYellow)] = "dyellow",
            [nameof(Gray)] = "gray",
            [nameof(DarkGray)] = "dgray",
            [nameof(Blue)] = "blue",
            [nameof(Green)] = "green",
            [nameof(Cyan)] = "cyan",
            [nameof(Red)] = "red",
            [nameof(Magenta)] = "magenta",
            [nameof(Yellow)] = "yellow",
            [nameof(White)] = "white"
        };
    }
}
