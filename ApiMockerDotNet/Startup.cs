using System;
using ApiMockerDotNet.Middlewares;
using ApiMockerDotNet.Repositories;
using ApiMockerDotNet.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApiMockerDotNet
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IFileSettingsProvider, FileSettingsProvider>();
            //services.AddSingleton<IApiMockerConfigRepository, ApiMockerConfigRepository>();
            services.AddTransient<IApiMockerConfigRepository, ApiMockerConfigRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!ApiMockerCmdParams.Quiet)
            {
                loggerFactory.AddProvider(new ColoredConsoleLoggerProvider(LogLevel.Warning, ConsoleColor.DarkYellow));
                loggerFactory.AddProvider(new ColoredConsoleLoggerProvider(LogLevel.Error, ConsoleColor.Red));
                loggerFactory.AddProvider(new ColoredConsoleLoggerProvider(LogLevel.Information, Console.ForegroundColor));
            }
         
            app.UseMiddleware<CallsInterceptorMiddleware>();
        }
    }
}
