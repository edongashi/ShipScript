namespace ShipScript.RShipCore.Pipes
{
    interface IAsyncWritableStream : IWritableStream
    {
        [NativeObjectHint("Promise")]
        object PromiseWrite(object value);
    }
}
