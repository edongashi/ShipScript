using System;
using ShipScript.Common;
using ShipScript.RShipCore.Bootstrappers.StandardIO;
using System.IO;
using ShipScript.RShipCore.Helpers;

namespace ShipScript.RShipCore.Bootstrappers
{
    public class RShipLoader : IScriptEngineBootstrapper
    {
        public string Name => RShipCore.CoreName;

        public string Version => RShipCore.CoreVersion;

        public string Run(IScriptEngine engine, string[] args)
        {
            var core = CreateCore(engine);
            var evaluator = core.Evaluator;
            var stdin = new StandardInputStream(evaluator, ConsoleColor.Yellow);
            core.AddNativeModule("stdin", stdin);
            SetOutputs(core);
            if (args == null || args.Length == 0)
            {
                core.Sleep();
            }
            else if (args.Length == 1)
            {
                var path = "/" + args[0];
                try
                {
                    core.Run(path);
                }
                catch
                {
                    core.Sleep();
                }
            }
            else if (args.Length > 1)
            {
                evaluator.Execute("args = []");
                var init = string.Empty;
                var length = args.Length;
                for (var i = 0; i < length; i++)
                {
                    var arg = args[i];
                    var name = $"arg{i + 1}";
                    engine.Script[name] = arg;
                    init += $"args.push({name});";
                }

                evaluator.Execute(init);
            }

            if (core.Sleeping)
            {
                core.ExposeGlobalRequire();
                core.ExposeExplore();
                var pipe = stdin.Pipe(core.CommandPipe);
                stdin.Run();
            }

            return null;
        }

        private RShipCore CreateCore(IScriptEngine engine)
        {
            var modulesPath = Path.Combine(PathHelpers.GetAssemblyDirectory(), "ship_modules");
            var pathResolver = new ModulePathResolver(modulesPath, new[] { ".ship", ".js", ".json", ".dll" }, "lib");
            var loaderFactory = new ModuleLoaderFactory();
            var core = new RShipCore(engine, pathResolver, loaderFactory);
            return core;
        }

        private void SetOutputs(RShipCore core)
        {
            core.StdOut.Writer = new StandardOutputWriter();
            var console = core.Console;
            console.ConsoleReader = new StandardInputReader();
            console.CoreStream.Pipe(new StandardOutputStream(ConsoleColor.Cyan));
            console.LogStream.Pipe(new StandardOutputStream(ConsoleColor.White));
            console.ResultStream.Pipe(new StandardOutputStream(ConsoleColor.Green));
            console.ErrStream.Pipe(new StandardOutputStream(ConsoleColor.Red));
        }
    }
}