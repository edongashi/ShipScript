namespace ShipScript.Launcher
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var bootstrapper = new RShipCore.Bootstrappers.RShipLoader();
            bootstrapper.Run(args);
        }
    }
}
