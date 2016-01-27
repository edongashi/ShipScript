using System.Collections.Generic;
using ShipScript.Common;

namespace ShipScript.RShipCore.NativeTypes
{
    [NativeObject("Array")]
    public class JArray : IScriptNativeObject
    {
        private readonly object array;

        public JArray(IScriptEvaluator evaluator)
            : this (evaluator, null)
        {
        }

        public JArray(IScriptEvaluator evaluator, IEnumerable<object> items)
        {
            dynamic scriptObject = evaluator.Evaluate("[]");
            if (items == null)
            {
                return;
            }

            foreach (var item in items)
            {
                scriptObject.push(item);
            }

            array = scriptObject;
        }

        [NativeObject("Array")]
        public object GetScriptObject() => array;
    }
}
