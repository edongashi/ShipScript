namespace ShipScript.RShipCore
{
    public interface IVirtualPath
    {
        string Identifier { get; }

        string ResolvePath();

        string ResolveDirectory();

        string ResolveExtension();

        string ResolveContent();
    }
}
