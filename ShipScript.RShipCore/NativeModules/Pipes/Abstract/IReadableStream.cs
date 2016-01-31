using System.Collections.Generic;
using ShipScript.RShipCore.EventModel;

namespace ShipScript.RShipCore.Pipes
{
    public interface IReadableStream : IEventSource
    {
        int PipeCount { get; }

        IEnumerable<IPipe> Pipes { get; }

        IPipe Pipe(object output);

        IPipe Pipe(object output, object transformFunction);

        [NativeObjectHint("Array")]
        object GetPipes();
        
        [NativeObjectHint("Promise")]
        object ReadNext();
    }
}
