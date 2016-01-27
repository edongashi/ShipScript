using System;

namespace ShipScript.Common
{
    public class CodeExecutionEventArgs : EventArgs
    {
        public CodeExecutionEventArgs(string code)
        {
            Code = code;
        }

        public string Code { get; }
    }
}
