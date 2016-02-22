using ShipScript.RShipCore.VirtualConsole;
using Console = System.Console;

namespace ShipScript.RShipCore.Bootstrappers.StandardIO
{
    public class StandardInputReader : IConsoleInput
    {
        public string Read()
        {
            return Console.ReadLine();
        }
    }
}
