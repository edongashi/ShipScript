using System;
using ShipScript.Common;
using ShipScript.RShipCore.Extensions;

namespace ShipScript.RShipCore
{
    public partial class RShipCore
    {
        [ScriptMember("unwrap")]
        public object Unwrap(object obj) => obj;

        [ScriptMember("eval")]
        public bool ScriptEvaluate()
        {
            try
            {
                engine.Execute("eval", @"EngineInternal.evalResult = eval(EngineInternal.evalCode)");
                return true;
            }
            catch (Exception ex)
            {
                engine.Script.EngineInternal.evalError = ex.GetScriptStack();
                return false;
            }
        }

        [ScriptMember("sleep")]
        public void Sleep() => Sleeping = true;
    }
}
