using System;
using ShipScript.RShipCore.EventModel;

namespace ShipScript.RShipCore.Pipes
{
    public interface IPipe : IEventConnection
    {
        IReadableStream Source { get; }
        
        IWritableStream Target { get; }

        [NativeObjectHint]
        object TransformFunction { get; set; }

        void Write(object value);

        void BreakPipe();
    }
}
