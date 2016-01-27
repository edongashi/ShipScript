namespace ShipScript.RShipCore.EventModel
{
    public interface IEventSource
    {
        IEventConnection Connect(object callback);
    }
}
