namespace ShipScript.Common
{
    public interface IScriptEngineExtension
    {
        string Name { get; }

        string Version { get; }

        IScriptEngine ScriptEngine { get; }

        void Bind(IScriptEngine engine);
    }
}
