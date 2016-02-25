using System;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using ShipScript.RShipCore.Pipes;

namespace ShipScript.RShipCore.Bootstrappers.StandardIO
{
    public class StandardInputStream : BaseReadableStream
    {
        private readonly ConsoleColor decoratorColor;

        public StandardInputStream(V8ScriptEngine evaluator, ConsoleColor decoratorColor)
            : base(evaluator)
        {
            this.decoratorColor = decoratorColor;
        }

        [ScriptMember("running")]
        public bool Running { get; private set; }

        [ScriptMember("run")]
        public void Run()
        {
            if (Running)
            {
                return;
            }

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

        [ScriptMember("stop")]
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
