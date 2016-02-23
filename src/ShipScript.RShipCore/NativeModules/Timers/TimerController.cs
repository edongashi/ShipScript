using System;
using System.Threading;
using ShipScript.Common;
using ShipScript.RShipCore.Extensions;
using ShipScript.RShipCore.Pipes;

namespace ShipScript.RShipCore.Timers
{
    [ModuleExports]
    public class TimerController
    {
        private readonly IWritableStream errorStream;

        public TimerController(IWritableStream errorStream)
        {
            this.errorStream = errorStream;
        }

        [ScriptMember("delay")]
        public void Delay(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        [ScriptMember("setInterval")]
        public IntervalController SetInterval(object callback, int milliseconds)
        {
            return InitTimer(callback, milliseconds, true);
        }

        [ScriptMember("setTimeout")]
        public IntervalController SetTimeout(object callback, int milliseconds)
        {
            return InitTimer(callback, milliseconds, false);
        }

        private IntervalController InitTimer(object callback, int milliseconds, bool repeat)
        {
            var interval = repeat ? milliseconds : Timeout.Infinite;
            var controller = new IntervalController(callback, interval);
            var timer = new Timer(TimerTick, controller, Timeout.Infinite, Timeout.Infinite);
            controller.Timer = timer;
            timer.Change(milliseconds, Timeout.Infinite);
            return controller;
        }

        private void TimerTick(object state)
        {
            var controller = (IntervalController)state;
            try
            {
                controller.Tick();
            }
            catch (Exception ex)
            {
                var stack = ex.GetScriptStack();
                errorStream?.Write(stack);
            }
        }
    }
}
