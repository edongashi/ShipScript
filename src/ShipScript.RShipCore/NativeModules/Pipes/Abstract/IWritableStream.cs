using Microsoft.ClearScript;

namespace ShipScript.RShipCore.Pipes
{
    public interface IWritableStream
    {
        [ScriptMember("write")]
        void Write(object value);
    }
}
