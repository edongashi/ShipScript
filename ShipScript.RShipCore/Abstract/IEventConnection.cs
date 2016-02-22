using ShipScript.Common;

namespace ShipScript.RShipCore
{
    public interface IEventConnection
    {
        [ScriptMember("disconnect")]
        void Disconnect();
    }
}
