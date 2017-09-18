using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiMockerDotNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            args = new string[] { "--c:sample-config.json" };
            int portNumber = 5000;
            if (args.Any())
            {
                var config = args.FirstOrDefault(x => x.StartsWith("--c:", StringComparison.OrdinalIgnoreCase))?.Replace("--c:", string.Empty);
                var port = args.FirstOrDefault(x => x.StartsWith("--p:", StringComparison.OrdinalIgnoreCase))?.Replace("--p:", string.Empty);
                var quiet = args.FirstOrDefault(x => x.StartsWith("--q:", StringComparison.OrdinalIgnoreCase))?.Replace("--q:", string.Empty);

                if (config != null)
                {
                    ApiMockerCmdParams.ConfigFile = config;
                }

                if (port != null)
                {
                    int.TryParse(port, out portNumber);
                    ApiMockerCmdParams.Port = portNumber;
                }

                if (quiet != null)
                {
                    bool.TryParse(quiet, out var isQuiet);
                    ApiMockerCmdParams.Quiet = isQuiet;
                }
            }

            return WebHost.CreateDefaultBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls($"http://localhost:{portNumber}/")
                .ConfigureLogging(x => x.ClearProviders())
                .Build();
        }
    }
}
