using Microsoft.ClearScript;

namespace ShipScript.RShipCore.Pipes
{
    public interface IAsyncWritableStream : IWritableStream
    {
        [ScriptMember("writeAsync")]
        [NativeObjectHint("Promise")]
        object PromiseWrite(object value);
    }
}
