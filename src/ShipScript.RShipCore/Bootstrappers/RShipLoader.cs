using System;
using System.IO;
using ShipScript.RShipCore.Bootstrappers.StandardIO;
using ShipScript.RShipCore.Helpers;

namespace ShipScript.RShipCore.Bootstrappers
{
    public class RShipLoader
    {
        public string Run(string[] args)
        {
            PrepareConsole();
            var core = CreateCore();
            var engine = core.Engine;
            var evaluator = core.Engine;
            var stdin = new StandardInputStream(evaluator, ConsoleColor.Green);
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
                if (core.ReplOnSleep)
                {
                    core.ExposeGlobalRequire();
                    engine.Execute("require('repl').hook(require('stdin'))");
                }

                stdin.Run();
            }

            core.Dispose();
            return null;
        }

        public void PrepareConsole()
        {
            var clearRequired = false;
            if (Console.BackgroundColor != ConsoleColor.Black)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                clearRequired = true;
            }

            if (Console.ForegroundColor != ConsoleColor.Gray)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                clearRequired = true;
            }

            if (clearRequired)
            {
                Console.Clear();
            }
        }

        public RShipCore CreateCore()
        {
            var modulesPath = Path.Combine(PathHelpers.GetAssemblyDirectory(), "ship_modules");
            var pathResolver = new ModulePathResolver(modulesPath, new[] { ".ship", ".js", ".json", ".dll" }, "index");
            var loaderFactory = new ModuleLoaderFactory();
            var core = new RShipCore(pathResolver, loaderFactory);
            return core;
        }

        private void SetOutputs(RShipCore core)
        {
            core.StdOut.Writer = new StandardOutputWriter();
            var console = core.Console;
            console.ConsoleReader = new StandardInputReader();
            console.CoreStream.Pipe(new StandardOutputStream(ConsoleColor.Cyan));
            console.LogStream.Pipe(new StandardOutputStream(ConsoleColor.White));
            console.ErrStream.Pipe(new StandardErrorStream(ConsoleColor.Red));
            console.Clearing += (s, e) => Console.Clear();
        }
    }
}
