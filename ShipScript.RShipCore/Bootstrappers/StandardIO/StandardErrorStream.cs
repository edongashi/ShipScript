using System;
using ShipScript.RShipCore.Extensions;
using ShipScript.RShipCore.Pipes;

namespace ShipScript.RShipCore.Bootstrappers.StandardIO
{
    public class StandardErrorStream : IWritableStream
    {
        public StandardErrorStream(ConsoleColor color)
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

            var newline = output.IndexOf('\n');
            string firstLine;
            string rest = null;
            if (newline != -1)
            {
                firstLine = output.Substring(0, newline);
                rest = output.Substring(newline);
            }
            else
            {
                firstLine = output;
            }

            var index = firstLine.IndexOf("Error:", StringComparison.Ordinal);
            string type = null;
            if (index != -1)
            {
                type = firstLine.Substring(0, index + 6);
                Console.Write(type);
                PrintColored(firstLine.Substring(index + 6));
            }

            if (type == null)
            {
                PrintColored(firstLine);
            }

            if (rest != null)
            {
                Console.WriteLine(rest);
            }
            else
            {
                Console.WriteLine();
            }
        }

        private void PrintColored(string text)
        {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.Write(text);
            Console.ForegroundColor = previous;
        }
    }
}
