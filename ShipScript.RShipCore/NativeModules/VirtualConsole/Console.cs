using System;
using System.Collections.Generic;
using ShipScript.Common;
using ShipScript.RShipCore.Pipes;
using ShipScript.RShipCore.Pipes.TransferModel;

namespace ShipScript.RShipCore.VirtualConsole
{
    [ModuleExports]
    public class Console : ICompositeStream<IPipeableStream>
    {
        private readonly IScriptEvaluator evaluator;

        public Console(IConsoleInput consoleReader, IScriptEvaluator evaluator)
        {
            this.evaluator = evaluator;
            ConsoleReader = consoleReader;
            DataStream = new PipeableStream(evaluator);
            AltStream = new PipeableStream(evaluator);
            LogStream = new PipeableStream(evaluator);
            ErrStream = new PipeableStream(evaluator);
            CoreStream = new PipeableStream(evaluator);
            CommandStream = new PipeableStream(evaluator);
            ResultStream = new PipeableStream(evaluator);
        }

        public IConsoleInput ConsoleReader { get; set; }

        public IPipeableStream DataStream { get; }
        public IPipeableStream AltStream { get; }
        public IPipeableStream LogStream { get; }
        public IPipeableStream ErrStream { get; }
        public IPipeableStream CoreStream { get; }
        public IPipeableStream CommandStream { get; }
        public IPipeableStream ResultStream { get; }

        public string Read()
        {
            if (ConsoleReader == null)
            {
                throw new InvalidOperationException("Current console has no readable input.");
            }

            return ConsoleReader.Read();
        }

        public void WriteData(object value)
        {
            DataStream.Write(value);
        }

        public void WriteAlt(object value)
        {
            AltStream.Write(value);
        }

        public void WriteLog(object value)
        {
            LogStream.Write(value);
        }

        public void WriteErr(object value)
        {
            ErrStream.Write(value);
        }

        public void WriteCore(object value)
        {
            CoreStream.Write(value);
        }

        public void WriteCommand(object value)
        {
            CommandStream.Write(value);
        }

        public void WriteResult(object value)
        {
            ResultStream.Write(value);
        }

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>()
        {
            [nameof(WriteData)] = "data",
            [nameof(WriteAlt)] = "alt",
            [nameof(WriteLog)] = "log",
            [nameof(WriteErr)] = "err",
            [nameof(WriteCore)] = "core",
            [nameof(WriteCommand)] = "command",
            [nameof(WriteResult)] = "result",
            [nameof(Read)] = "read"
        };
    }
}
