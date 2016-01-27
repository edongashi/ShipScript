using System;

namespace ShipScript.RShipCore.Pipes
{
    public class Pipe : IPipe
    {
        private readonly Action<Pipe> breakFunction;

        public Pipe(IReadableStream source, IWritableStream target, object transformFunction, Action<Pipe> breakFunction)
        {
            Source = source;
            Target = target;
            TransformFunction = transformFunction;
            this.breakFunction = breakFunction;
        }

        public IReadableStream Source { get; }

        public IWritableStream Target { get; }

        [NativeObjectHint("Function")]
        public dynamic TransformFunction { get; set; }

        public bool Broken { get; private set; }

        public void Write(object value)
        {
            if (Broken)
            {
                throw new PipeBrokenException(this);
            }

            try
            {
                var transformFunction = TransformFunction;
                if (transformFunction != null)
                {
                    object result = transformFunction(value);
                    if (result is CancelToken)
                    {
                        return;
                    }

                    Target.Write(result);
                }
                else
                {
                    Target.Write(value);
                }
            }
            catch
            {
                // ignored
            }
        }

        public void BreakPipe()
        {
            if (Broken)
            {
                return;
            }

            breakFunction(this);
            Broken = true;
        }

        public void Disconnect()
        {
            BreakPipe();
        }
    }
}
