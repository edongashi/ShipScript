using System.Collections.Generic;
using System.Linq;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
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

        protected readonly V8ScriptEngine Evaluator;

        protected BaseReadableStream(V8ScriptEngine evaluator)
        {
            Evaluator = evaluator;
            pipes = new List<IPipe>();
            promises = new List<Promise>();
        }

        [ScriptMember("pipeCount")]
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

        [ScriptMember("connect")]
        public IEventConnection Connect(object callback)
        {
            return Pipe(callback);
        }

        [ScriptMember("pipe")]
        public virtual IPipe Pipe(object output)
        {
            return Pipe(output, null);
        }

        [ScriptMember("pipe")]
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

        [ScriptMember("getPipes")]
        [NativeObject("Array")]
        public object GetPipes()
        {
            lock (syncRoot)
            {
                var array = new JArray(Evaluator, pipes);
                return array.GetScriptObject();
            }
        }

        [ScriptMember("next")]
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
                // ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 0; i < pipes.Count; i++)
                {
                    try
                    {
                        var pipe = pipes[i];
                        pipe.Write(value);
                    }
                    catch (PipeBrokenException)
                    {
                    }
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
    }
}
