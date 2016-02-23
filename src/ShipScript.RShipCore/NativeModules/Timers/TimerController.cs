using System.Threading;
using System.Threading.Tasks;
using ShipScript.Common;

namespace ShipScript.RShipCore.Timers
{
    [ModuleExports]
    public class TimerController
    {
        [ScriptMember("delay")]
        public void Delay(int milliseconds)
        {
            Task.Delay(milliseconds).Wait();
        }

        [ScriptMember("setInterval")]
        public void SetInterval(object callback, int milliseconds)
        {
            var timer = new System.Threading.Timer(DynamicCallback, callback, milliseconds, Timeout.Infinite);
        }

        [ScriptMember("setTimeout")]
        public void SetTimeout(int milliseconds, object callback)
        {
            Task.Delay(milliseconds).Wait();
        }

        private void DynamicCallback(object callback)
        {
            ((dynamic)callback)();
        } 
    }
}
