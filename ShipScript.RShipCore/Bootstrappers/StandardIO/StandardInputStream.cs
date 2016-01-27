using System;
using ShipScript.Common;
using ShipScript.RShipCore.Pipes;

namespace ShipScript.RShipCore.Bootstrappers.StandardIO
{
    public class StandardInputStream : BaseReadableStream
    {
        private readonly ConsoleColor decoratorColor;

        public StandardInputStream(IScriptEvaluator evaluator, ConsoleColor decoratorColor)
            : base(evaluator)
        {
            this.decoratorColor = decoratorColor;
        }

        public bool Running { get; private set; }

        public void Run()
        {
            Running = true;
            while (Running)
            {
                var previous = Console.ForegroundColor;
                Console.ForegroundColor = decoratorColor;
                Console.Write("> ");
                Console.ForegroundColor = previous;
                var value = Console.ReadLine();
                if (Running)
                {
                    Write(value);
                }
            }
        }

        public void Stop()
        {
            if (!Running)
            {
                return;
            }

            Running = false;
        }
    }
}
