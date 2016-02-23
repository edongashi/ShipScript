namespace ShipScript.Common
{
    public interface IScriptEngineBootstrapper
    {
        string Run(IScriptEngine engine, string[] args);
    }
}
