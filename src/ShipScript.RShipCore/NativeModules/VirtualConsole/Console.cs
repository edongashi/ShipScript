using System;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using ShipScript.RShipCore.Pipes;
using ShipScript.RShipCore.Pipes.TransferModel;

namespace ShipScript.RShipCore.VirtualConsole
{
    [ModuleExports]
    public class Console : ICompositeStream<IPipeableStream>
    {
        private readonly V8ScriptEngine evaluator;

        public Console(IConsoleInput consoleReader, V8ScriptEngine evaluator)
        {
            this.evaluator = evaluator;
            ConsoleReader = consoleReader;
            DataStream = new PipeableStream(evaluator);
            AltStream = new PipeableStream(evaluator);
            LogStream = new PipeableStream(evaluator);
            ErrorStream = new PipeableStream(evaluator);
            CoreStream = new PipeableStream(evaluator);
            CommandStream = new PipeableStream(evaluator);
            ResultStream = new PipeableStream(evaluator);
        }

        public event EventHandler Clearing;

        public IConsoleInput ConsoleReader { get; set; }

        [ScriptMember("dataStream")]
        public IPipeableStream DataStream { get; }

        [ScriptMember("altStream")]
        public IPipeableStream AltStream { get; }

        [ScriptMember("logStream")]
        public IPipeableStream LogStream { get; }

        [ScriptMember("errorStream")]
        public IPipeableStream ErrorStream { get; }

        [ScriptMember("coreStream")]
        public IPipeableStream CoreStream { get; }

        [ScriptMember("commandStream")]
        public IPipeableStream CommandStream { get; }

        [ScriptMember("resultStream")]
        public IPipeableStream ResultStream { get; }

        [ScriptMember("read")]
        public string Read()
        {
            if (ConsoleReader == null)
            {
                throw new InvalidOperationException("Current console has no readable input.");
            }

            return ConsoleReader.Read();
        }

        [ScriptMember("data")]
        public void WriteData(object value)
        {
            DataStream.Write(value);
        }

        [ScriptMember("alt")]
        public void WriteAlt(object value)
        {
            AltStream.Write(value);
        }

        [ScriptMember("log")]
        public void WriteLog(object value)
        {
            LogStream.Write(value);
        }

        [ScriptMember("error")]
        public void WriteError(object value)
        {
            ErrorStream.Write(value);
        }

        [ScriptMember("core")]
        public void WriteCore(object value)
        {
            CoreStream.Write(value);
        }

        [ScriptMember("command")]
        public void WriteCommand(object value)
        {
            CommandStream.Write(value);
        }

        [ScriptMember("result")]
        public void WriteResult(object value)
        {
            ResultStream.Write(value);
        }

        [ScriptMember("clear")]
        public void Clear()
        {
            Clearing?.Invoke(this, EventArgs.Empty);
        }
    }
}
