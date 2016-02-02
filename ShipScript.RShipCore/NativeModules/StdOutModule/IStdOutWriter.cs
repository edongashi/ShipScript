namespace ShipScript.RShipCore.StdOutModule
{
    public interface IStdOutWriter
    {
        void Write(string value);

        void Write(string value, int color);

        void WriteLine();

        void WriteLine(string value);

        void WriteLine(string value, int color);
    }
}
