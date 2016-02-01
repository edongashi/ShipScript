namespace ShipScript.RShipCore
{
    public interface IReflectable
    {
        object CreateObject(string name, params object[] parameters);
    }
}
