namespace ShipScript.RShipCore
{
    public interface IModulePathResolver
    {
        IVirtualPath Resolve(string path, Module parent);
    }
}
