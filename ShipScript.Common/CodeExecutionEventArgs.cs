namespace ShipScript.Common
{
    public class CodeExecutionEventArgs
    {
        public CodeExecutionEventArgs(string code)
        {
            Code = code;
        }

        public string Code { get; }
    }
}
