namespace ShipScript.RShipCore
{
    public interface IEventSource
    {
        IEventConnection Connect(object callback);
    }
}
