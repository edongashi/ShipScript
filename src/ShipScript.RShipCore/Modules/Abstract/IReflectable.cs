using System;

namespace ShipScript.RShipCore
{
    [Obsolete]
    public interface IReflectable
    {
        object CreateObject(string name, params object[] parameters);
    }
}
