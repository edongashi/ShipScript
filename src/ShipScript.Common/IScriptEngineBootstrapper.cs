namespace ShipScript.Common
{
    public interface IScriptEngineBootstrapper
    {
        string Name { get; }

        string Version { get; }

        string Run(IScriptEngine engine, string[] args);
    }
}
