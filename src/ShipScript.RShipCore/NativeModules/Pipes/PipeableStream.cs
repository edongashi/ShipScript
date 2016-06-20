using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using ShipScript.RShipCore.Extensions;

namespace ShipScript.RShipCore.Pipes
{
    public class PipeableStream : ReadableStream, IPipeableStream
    {
        public PipeableStream(V8ScriptEngine evaluator)
            : base(evaluator)
        {
        }

        [ScriptMember("write")]
        public override void Write(object value)
        {
            var newValue = OnWriting(value);
            if (newValue is CancelToken)
            {
                return;
            }

            if (newValue.IsDefined() && !(newValue is IgnoreToken))
            {
                base.Write(newValue);
                OnWritten(newValue);
            }
            else
            {
                base.Write(value);
                OnWritten(value);
            }
        }

        protected virtual object OnWriting(object value)
        {
            return IgnoreToken.Instance;
        }

        protected virtual void OnWritten(object value)
        {
        }
    }
}
