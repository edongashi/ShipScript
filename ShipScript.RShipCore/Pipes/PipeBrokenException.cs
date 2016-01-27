using System;

namespace ShipScript.RShipCore.Pipes
{
    public class PipeBrokenException : Exception
    {
        public PipeBrokenException(IPipe pipe)
        {
            Pipe = pipe;
        }

        public IPipe Pipe { get; }
    }
}
