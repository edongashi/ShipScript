using System;

namespace ShipScript.Common
{
    public interface IScriptEngine : IScriptEvaluator, IDisposable
    {
        dynamic Script { get; }

        ScriptAccess DefaultAccess { get; set; }

        Func<bool> ContinuationCallback { get; set; }

        void AddHostObject(string itemName, object target);

        string ExecuteCommand(string command);

        object Invoke(string funcName, params object[] args);

        void Interrupt();

        string GetStackTrace();

        void CollectGarbage(bool exhaustive);

        object CreateHostFunctions();

        object CreateExtendedHostFunctions();
    }
}
