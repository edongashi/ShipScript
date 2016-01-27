using System;
using System.Collections.Generic;
using ShipScript.Common;
using ShipScript.RShipCore.Extensions;
using ShipScript.RShipCore.NativeTypes;

namespace ShipScript.RShipCore.Pipes
{
    public class PipeableStream : BaseReadableStream, IPipeableStream
    {
        public PipeableStream(IScriptEvaluator evaluator)
            : base(evaluator)
        { }

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

            if (Evaluator.IsDefined(newValue) && !(newValue is IgnoreToken))
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
