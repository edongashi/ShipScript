using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using ShipScript.RShipCore.NativeTypes;

namespace ShipScript.RShipCore
{
    [ModuleExports]
    public class Await
    {
        private readonly V8ScriptEngine evaluator;

        public Await(V8ScriptEngine evaluator)
        {
            this.evaluator = evaluator;
        }

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
    }
}
