using System;
using Microsoft.ClearScript.V8;
using ShipScript.RShipCore.Extensions;

namespace ShipScript.RShipCore.Pipes
{
    public class PipeableStream : BaseReadableStream, IPipeableStream
    {
        public PipeableStream(V8ScriptEngine evaluator)
            : base(evaluator)
        {
        }

        public override IPipe Pipe(object output)
        {
            if (output == this)
            {
                throw new InvalidOperationException("Cannot pipe object to itself.");
            }

            return base.Pipe(output);
        }

        public override IPipe Pipe(object output, object transformFunction)
        {
            if (output == this)
            {
                throw new InvalidOperationException("Cannot pipe object to itself.");
            }

            return base.Pipe(output, transformFunction);
        }

        public new void Write(object value)
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
