namespace ShipScript.RShipCore.Pipes
{
    public class CallbackWritableStream : IWritableStream
    {
        [NativeObjectHint("Function")]
        public dynamic Callback { get; set; }

        public void Write(object value)
        {
            Callback(value);
        }
    }
}
