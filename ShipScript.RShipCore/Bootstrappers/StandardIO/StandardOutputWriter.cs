using System;
using ShipScript.RShipCore.AbstractStdOut;
using ShipScript.RShipCore.StdOut;
using ShipScript.RShipCore.StdOutModule;

namespace ShipScript.RShipCore.Bootstrappers.StandardIO
{
    public class StandardOutputWriter : IStdOutWriter
    {
        public void Write(string value)
        {
            Console.Write(value);
        }

        public void Write(string value, int color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = (ConsoleColor)color;
            Console.Write(value);
            Console.ForegroundColor = previousColor;
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public void WriteLine(string value, int color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = (ConsoleColor)color;
            Console.WriteLine(value);
            Console.ForegroundColor = previousColor;
        }
    }
}
