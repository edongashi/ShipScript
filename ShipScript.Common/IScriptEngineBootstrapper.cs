namespace ShipScript.Common
{
    public interface IScriptEngineBootstrapper
    {
        string Name { get; }

        string Version { get; }

        IScriptEngine ScriptEngine { get; }

        void Bind(IScriptEngine engine, string[] args);

        string Run();
    }
}
