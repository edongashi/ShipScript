using System;
using ShipScript.RShipCore.Extensions;
using ShipScript.RShipCore.Pipes;

namespace ShipScript.RShipCore.Bootstrappers.StandardIO
{
    public class StandardOutputStream : IWritableStream
    {
        public StandardOutputStream(ConsoleColor color)
        {
            Color = color;
        }

        public ConsoleColor Color { get; set; }

        public void Write(object value)
        {
            var output = value.ToScriptString();
            if (output == null)
            {
                return;
            }

            var previous = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.WriteLine(output);
            Console.ForegroundColor = previous;
        }
    }
}
