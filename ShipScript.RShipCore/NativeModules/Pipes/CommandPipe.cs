using System.Collections.Generic;

namespace ShipScript.RShipCore.Pipes
{
    public class CommandPipe : IWritableStream
    {
        private readonly ICommandReceiver receiver;

        public CommandPipe(ICommandReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Write(object value)
        {
            var command = value as string;
            if (command != null)
            {
                receiver.ExecuteCommand(command);
            }
        }

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>
        {
            [nameof(Write)] = "write"
        };
    }
}
