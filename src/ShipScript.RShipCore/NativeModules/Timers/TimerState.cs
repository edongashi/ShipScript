using System.Threading;

namespace ShipScript.RShipCore.Timers
{
    class TimerState
    {
        public TimerState(Timer timer, object callback)
        {
            Timer = timer;
            Callback = callback;
        }

        public Timer Timer { get; }

        public object Callback { get; }

        public void InvokeCallback() => ((dynamic)Callback)();
    }
}
