using System.Threading.Tasks;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using ShipScript.RShipCore.NativeTypes;

namespace ShipScript.RShipCore
{
    [ModuleExports]
    public class Async : IScriptNativeObject
    {
        private object scriptObject;

        private readonly V8ScriptEngine evaluator;

        public Async(V8ScriptEngine evaluator)
        {
            this.evaluator = evaluator;
        }

        [ScriptMember("wait")]
        public T Wait<T>(Task<T> task)
        {
            return task.Result;
        }

        [ScriptMember("wait")]
        public object Wait(Task task)
        {
            task.Wait();
            return null;
        }

        [ScriptMember("promise")]
        [NativeObject("Promise")]
        public object PromiseFromTask<T>(Task<T> task)
        {
            var promise = new Promise(evaluator);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    promise.Reject(t.Exception);
                }
                else if (t.IsCanceled)
                {
                    promise.Reject("canceled");
                }
                else
                {
                    promise.Resolve(t.Result);
                }
            });

            return promise.GetScriptObject();
        }

        [ScriptMember("promise")]
        [NativeObject("Promise")]
        public object PromiseFromTask(Task task)
        {
            var promise = new Promise(evaluator);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    promise.Reject(t.Exception);
                }
                else if (t.IsCanceled)
                {
                    promise.Reject("canceled");
                }
                else
                {
                    promise.Resolve(new object[] { null });
                }
            });

            return promise.GetScriptObject();
        }

        [NativeObject("Function")]
        public object GetScriptObject()
        {
            if (scriptObject != null)
            {
                return scriptObject;
            }

            var func = evaluator.Evaluate("native", false, @"(
                function (nativeAsync) {
                    function async(task) { return nativeAsync.promise(task); }
                    async.wait = nativeAsync.wait.toFunction();
                    return async;
                }).valueOf()");

            scriptObject = ((dynamic)func)(this);
            return scriptObject;
        }
    }
}
