namespace ShipScript.Common
{
    public interface IScriptEvaluator
    {
        void Execute(string code);

        object Evaluate(string code);

        bool IsUndefined(object value);

        bool IsVoidResult(object value);
    }
}
