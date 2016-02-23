using System;
using System.Threading;
using ShipScript.Common;
using ShipScript.RShipCore.Extensions;

namespace ShipScript.RShipCore.Timers
{
    public class IntervalController
    {
        private bool disposed;

        public IntervalController(object callback, int interval)
        {
            Callback = callback;
            Interval = interval;
        }

        public Timer Timer { get; set; }

        public object Callback { get; }

        [ScriptMember("interval", ScriptAccess.ReadOnly)]
        public int Interval { get; }

        [ScriptMember("stopped", ScriptAccess.ReadOnly)]
        public bool Stopped { get; private set; }

        [ScriptMember("stop", ScriptAccess.ReadOnly)]
        public void Stop() => Stopped = true;

        public void Tick()
        {
            if (disposed)
            {
                return;
            }

            if (Stopped)
            {
                Timer.Dispose();
                disposed = true;
                return;
            }

            try
            {
                ((dynamic) Callback)();
            }
            catch
            {
                Timer.Dispose();
                disposed = true;
                throw;
            }

            if (Interval == Timeout.Infinite)
            {
                Timer.Dispose();
                disposed = true;
            }
            else
            {
                Timer.Change(Interval, Timeout.Infinite);
            }
        }
    }
}
