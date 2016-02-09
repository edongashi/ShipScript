using System;
using ShipScript.Common;

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

        [ScriptMember("source", ScriptMemberFlags.ExposeRuntimeType)]
        public IReadableStream Source { get; }

        [ScriptMember("target", ScriptMemberFlags.ExposeRuntimeType)]
        public IWritableStream Target { get; }

        [ScriptMember("transform")]
        [NativeObjectHint("Function")]
        public dynamic TransformFunction { get; set; }

        [ScriptMember("broken")]
        public bool Broken { get; private set; }

        [ScriptMember("write")]
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

        [ScriptMember("break")]
        public void BreakPipe()
        {
            if (Broken)
            {
                return;
            }

            breakFunction(this);
            Broken = true;
        }

        [ScriptMember("disconnect")]
        public void Disconnect()
        {
            BreakPipe();
        }
    }
}
