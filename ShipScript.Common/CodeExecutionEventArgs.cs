namespace ShipScript.Common
{
    public class CodeExecutionEventArgs
    {
        public CodeExecutionEventArgs(string code, ExecutionMethod executionMethod)
        {
            Code = code;
            ExecutionMethod = executionMethod;
        }

        public string Code { get; }
        
        public ExecutionMethod ExecutionMethod { get; }
    }
}
