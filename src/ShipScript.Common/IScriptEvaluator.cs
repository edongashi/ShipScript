namespace ShipScript.Common
{
    public interface IScriptEvaluator
    {
        void Execute(string code);

        void Execute(string documentName, string code);

        void Execute(string documentName, bool discard, string code);

        object Evaluate(string code);

        object Evaluate(string documentName, string code);

        object Evaluate(string documentName, bool discard, string code);
    }
}
