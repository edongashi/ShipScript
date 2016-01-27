using ShipScript.Common;

namespace ShipScript.RShipCore.Extensions
{
    public static class ScriptEvaluatorExtensions
    {
        public static bool IsDefined(this IScriptEvaluator evaluator, object value)
        {
            return !evaluator.IsUndefined(value) && !evaluator.IsVoidResult(value);
        }
    }
}
