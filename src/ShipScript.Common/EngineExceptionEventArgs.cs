using System;

namespace ShipScript.Common
{
    public class EngineExceptionEventArgs : EventArgs
    {
        public EngineExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}
