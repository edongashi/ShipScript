namespace ShipScript.RShipCore.Pipes
{
    public interface IAsyncWritableStream : IWritableStream
    {
        [NativeObjectHint("Promise")]
        object PromiseWrite(object value);
    }
}
