using System;

namespace ShipScript.Common
{
    public interface IScriptEngine : IDisposable
    {
        event EventHandler<CodeExecutionEventArgs> InteractiveCodeExecution;

        event EventHandler<EngineExceptionEventArgs> InteractiveExecutionException;

        event EventHandler<CodeExecutionEventArgs> CodeExecution;

        event EventHandler<EngineExceptionEventArgs> ExecutionException;

        event EventHandler ScriptInterrupted;

        event EventHandler Disposing;

        event EventHandler Disposed;

        string EngineName { get; }

        string EngineVersion { get; }

        string UnderlyingEngineName { get; }

        string UnderlyingEngineVersion { get; }

        string ScriptLanguage { get; }

        dynamic Script { get; }

        ScriptAccess DefaultScriptAccess { get; set; }

        Func<bool> ContinuationCallback { get; set; }
        
        string InteractiveExecuteCommand(string command);

        void InteractiveExecute(string code);

        object InteractiveEvaluate(string code);

        string ExecuteCommand(string command);

        void Execute(string code);

        object Evaluate(string code);

        void Invoke(string funcName, params object[] args);

        void Interrupt();

        string GetStackTrace();

        void CollectGarbage(bool exhaustive);
    }
}
