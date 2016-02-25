using System;
using System.Dynamic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;

namespace ShipScript.RShipCore.Helpers
{
    public static class ObjectHelpers
    {
        private static readonly CSharpArgumentInfo ArgInfo = CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,
            null);

        public static object DynamicInvoke(object target, params object[] args)
        {
            var del = target as Delegate;
            if (del != null)
            {
                return del.DynamicInvoke(args);
            }

            var dynamicObject = target as DynamicObject;
            if (dynamicObject == null)
            {
                throw new InvalidOperationException("Invocation failed");
            }

            object result;
            var binder = Binder.Invoke(CSharpBinderFlags.None, null, Enumerable.Repeat(ArgInfo, args.Length));
            if (dynamicObject.TryInvoke((InvokeBinder)binder, args, out result))
            {
                return result;
            }

            throw new InvalidOperationException("Invocation failed");
        }
    }
}
