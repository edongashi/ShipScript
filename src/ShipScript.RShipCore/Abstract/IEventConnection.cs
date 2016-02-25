using Microsoft.ClearScript;

namespace ShipScript.RShipCore
{
    public interface IEventConnection
    {
        [ScriptMember("disconnect")]
        void Disconnect();
    }
}
