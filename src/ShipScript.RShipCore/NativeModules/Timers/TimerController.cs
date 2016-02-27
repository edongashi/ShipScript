using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ClearScript;
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

        [ScriptMember("block")]
        public void Block(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        [ScriptMember("delay")]
        public Task Delay(int milliseconds)
        {
            return Task.Delay(milliseconds);
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

        [ScriptMember("setImmediate")]
        public void SetImmediate(object callback)
        {
            Task.Run(() => ((dynamic)callback)());
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
