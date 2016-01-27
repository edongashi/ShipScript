using System;
using ShipScript.Common;

namespace ShipScript.RShipCore.Pipes
{
    public class CallbackPipeableStream : PipeableStream
    {
        public CallbackPipeableStream(IScriptEvaluator evaluator, object callback)
            : base(evaluator)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            Callback = callback;
        }

        [NativeObjectHint("Function")]
        public dynamic Callback { get; set; }

        protected override object OnWriting(object value)
        {
            return Callback(value);
        }
    }
}
