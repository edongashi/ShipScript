using System;

namespace ShipScript.Common
{
    public interface IScriptEngine : IScriptEvaluator, IDisposable
    {
        event EventHandler<CodeExecutionEventArgs> ScriptExecuting;

        event EventHandler<CodeExecutionEventArgs> CommandExecuting;

        event EventHandler<CodeExecutionEventArgs> CommandExecuted;

        event EventHandler<EngineExceptionEventArgs> ScriptExecutionException;

        event EventHandler<EngineExceptionEventArgs> CommandExecutionException;

        event EventHandler ScriptExecutionInterrupted;

        event EventHandler CommandExecutionInterrupted;

        event EventHandler Disposing;

        event EventHandler Disposed;

        string EngineName { get; }

        string EngineVersion { get; }

        string UnderlyingEngineName { get; }

        string UnderlyingEngineVersion { get; }

        string ScriptLanguage { get; }

        dynamic Script { get; }

        ScriptAccessEnum DefaultScriptAccess { get; set; }

        Func<bool> ContinuationCallback { get; set; }

        void AddHostObject(string itemName, object target);

        string ExecuteCommand(string command);

        void Invoke(string funcName, params object[] args);

        void Interrupt();

        string GetStackTrace();

        void CollectGarbage(bool exhaustive);

        object CreateHostFunctions();

        object CreateExtendedHostFunctions();
    }
}
