using System;
using Microsoft.ClearScript.V8;
using ShipScript.RShipCore.Helpers;

namespace ShipScript.RShipCore.NativeTypes
{
    [NativeObject("Promise")]
    public class Promise : IScriptNativeObject
    {
        private readonly object promise;
        private object resolveCallback;
        private object rejectCallback;

        public Promise(V8ScriptEngine evaluator)
        {
            var bag = (dynamic)evaluator.Evaluate(@"
                (function () {
                    var bag = { };
                    bag.promise = new Promise(function (resolve, reject) {
                        bag.resolve = resolve;
                        bag.reject = reject;
                    });
                    return bag;
                })()");

            promise = bag.promise;
            resolveCallback = bag.resolve;
            rejectCallback = bag.reject;
            Pending = true;
        }

        public bool Pending { get; private set; }

        public bool Fulfilled { get; private set; }

        public bool Rejected { get; private set; }

        public bool Settled => Fulfilled || Rejected;

        public void Resolve(params object[] args)
        {
            if (Settled)
            {
                throw new InvalidOperationException("Promise has already been settled");
            }

            if (args == null || args.Length == 0)
            {
                ((dynamic)resolveCallback)();
            }
            else
            {
                ObjectHelpers.DynamicInvoke(resolveCallback, args);
            }

            // Release scipt object references
            resolveCallback = null;
            rejectCallback = null;
            Pending = false;
            Fulfilled = true;
        }

        public void Reject(object reason)
        {
            if (Settled)
            {
                throw new InvalidOperationException("Promise has already been settled");
            }

            ((dynamic)rejectCallback)(reason);
            resolveCallback = null;
            rejectCallback = null;
            Pending = false;
            Rejected = true;
        }

        [NativeObject("Promise")]
        public object GetScriptObject() => promise;
    }
}
