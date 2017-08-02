using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ApiMockerDotNet.Web.Utils;
using ApiMockerDotNet.Web.Filters;
using Microsoft.Extensions.Logging.Console;

namespace ApiMockerDotNet.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {                       
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(IgnoreSystemLogs);
            if (!ApiMockerCmdParams.Quiet)
            {
                loggerFactory.AddProvider(new ColoredConsoleLoggerProvider(LogLevel.Warning, ConsoleColor.DarkYellow));
                loggerFactory.AddProvider(new ColoredConsoleLoggerProvider(LogLevel.Error, ConsoleColor.Red));
                loggerFactory.AddProvider(new ColoredConsoleLoggerProvider(LogLevel.Information, Console.ForegroundColor));
            }
            var logger = loggerFactory.CreateLogger("ApiMockerLog");
            IApiMockerManager apiMockerManager = new ApiMockerManager(logger, ApiMockerCmdParams.ConfigFile);
            app.UseMiddleware<CallsInterceptorMiddleware>(apiMockerManager, logger);     
        }

        private bool IgnoreSystemLogs(string message, LogLevel logLevel)
        {
            if ((message.Contains("Request starting") || message.Contains("Request finished")) && logLevel == LogLevel.Information)
            {
                return true;
            }

            return false;
        }
    }
}
