using System.Collections.Generic;
using System.Linq;
using ShipScript.Common;
using ShipScript.RShipCore.EventModel;
using ShipScript.RShipCore.NativeTypes;

namespace ShipScript.RShipCore.Pipes
{
    public abstract class BaseReadableStream : IReadableStream
    {
        private readonly object syncRoot = new object();
        private readonly object removeSyncRoot = new object();
        private bool writing;
        private readonly List<IPipe> pipesToRemove = new List<IPipe>();

        private readonly List<IPipe> pipes;
        private readonly List<Promise> promises;

        protected readonly IScriptEvaluator Evaluator;

        protected BaseReadableStream(IScriptEvaluator evaluator)
        {
            Evaluator = evaluator;
            pipes = new List<IPipe>();
            promises = new List<Promise>();
        }

        public int PipeCount => pipes.Count;

        public IEnumerable<IPipe> Pipes
        {
            get
            {
                lock (syncRoot)
                {
                    return pipes.ToList();
                }
            }
        }

        public IEventConnection Connect(object callback)
        {
            return Pipe(callback);
        }

        public virtual IPipe Pipe(object output)
        {
            return Pipe(output, null);
        }

        public virtual IPipe Pipe(object output, object transformFunction)
        {
            var writableStream = output as IWritableStream ?? new CallbackPipeableStream(Evaluator, output);
            var pipe = new Pipe(this, writableStream, transformFunction, RemovePipe);
            lock (syncRoot)
            {
                pipes.Add(pipe);
            }

            return pipe;
        }

        [NativeObject("Array")]
        public object GetPipes()
        {
            lock (syncRoot)
            {
                var array = new JArray(Evaluator, pipes);
                return array.GetScriptObject();
            }
        }

        [NativeObject("Promise")]
        public object ReadNext()
        {
            lock (syncRoot)
            {
                var promise = new Promise(Evaluator);
                promises.Add(promise);
                return promise.GetScriptObject();
            }
        }

        protected void Write(object value)
        {
            lock (syncRoot)
            {
                writing = true;
                foreach (var promise in promises)
                {
                    promise.Resolve(value);
                }

                promises.Clear();
                foreach (var pipe in pipes)
                {
                    pipe.Write(value);
                }

                lock (removeSyncRoot)
                {
                    if (pipesToRemove.Count != 0)
                    {
                        foreach (var pipe in pipesToRemove)
                        {
                            pipes.Remove(pipe);
                        }

                        pipesToRemove.Clear();
                    }

                    writing = false;
                }
            }
        }

        private void RemovePipe(IPipe pipe)
        {
            lock (removeSyncRoot)
            {
                if (writing)
                {
                    pipesToRemove.Add(pipe);
                    return;
                }
            }

            lock (syncRoot)
            {
                pipes.Remove(pipe);
            }
        }

        private static readonly Dictionary<string, string> ScriptAccess = new Dictionary<string, string>
        {
            { nameof(Pipe), "pipe" },
        };
    }
}
