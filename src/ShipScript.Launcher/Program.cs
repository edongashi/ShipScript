using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using ShipScript.RShipCore;

namespace ShipScript.Launcher
{
    public class Program
    {
        private const string Version = "1.0";

        static void Main(string[] args)
        {
            var engine = new V8ScriptEngine();
            engine.AllowReflection = true;
            var bootstrapper = new RShipCore.Bootstrappers.RShipLoader();
            bootstrapper.Run(engine, args);
        }
    }
}
