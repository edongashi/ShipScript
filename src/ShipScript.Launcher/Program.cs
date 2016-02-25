using Microsoft.ClearScript.V8;

namespace ShipScript.Launcher
{
    public class Program
    {
        static void Main(string[] args)
        {
            var engine = new V8ScriptEngine { AllowReflection = true };
            var bootstrapper = new RShipCore.Bootstrappers.RShipLoader();
            bootstrapper.Run(engine, args);
        }
    }
}
