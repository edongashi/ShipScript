using System;
using Microsoft.ClearScript;
using ShipScript.RShipCore.Extensions;

namespace ShipScript.RShipCore
{
    public partial class RShipCore
    {
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
                engine.Script.EngineInternal.lastError = ex;
                return false;
            }
        }

        [ScriptMember("sleep")]
        public object Sleep(bool startRepl = true)
        {
            Sleeping = true;
            ReplOnSleep = startRepl;
            Module stdIn;
            return NativeModules.TryGetValue("stdin", out stdIn) ? stdIn.Exports : null;
        }
    }
}
