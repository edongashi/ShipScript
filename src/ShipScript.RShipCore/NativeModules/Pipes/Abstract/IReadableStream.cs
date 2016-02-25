using System.Collections.Generic;
using Microsoft.ClearScript;

namespace ShipScript.RShipCore.Pipes
{
    public interface IReadableStream : IEventSource
    {
        [ScriptMember("pipeCount")]
        int PipeCount { get; }

        IEnumerable<IPipe> Pipes { get; }

        [ScriptMember("pipe")]
        IPipe Pipe(object output);

        [ScriptMember("pipe")]
        IPipe Pipe(object output, object transformFunction);

        [ScriptMember("getPipes")]
        [NativeObjectHint("Array")]
        object GetPipes();

        [ScriptMember("next")]
        [NativeObjectHint("Promise")]
        object ReadNext();
    }
}
