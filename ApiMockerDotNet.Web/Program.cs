using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ApiMockerDotNet.Web.Utils;

namespace ApiMockerDotNet.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //args = new string[] { "--c:sample-config.json" };
            int portNumber = 5000;
            if (args.Any())
            {
                var config = args.FirstOrDefault(x => x.StartsWith("--c:", StringComparison.OrdinalIgnoreCase))?.Replace("--c:",string.Empty);
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
                
                if (quiet!= null)
                {
                    bool isQuiet = false;
                    bool.TryParse(quiet, out isQuiet);
                    ApiMockerCmdParams.Quiet = isQuiet;
                }               
            }
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls($"http://localhost:{portNumber}/")
                .UseApplicationInsights()
                .Build();

            host.Run();          

        }
    }

    public static class ApiMockerCmdParams
    {
        public static string ConfigFile;
        public static int Port;
        public static bool Quiet;
    }
}
