using ApiMockerDotNet.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ApiMockerDotNet.Web.Entities;
using ApiMockerDotNet.Web.Repositories;

namespace ApiMockerDotNet.Web.Filters
{
    public class CallsInterceptorMiddleware
    {
        private readonly RequestDelegate next;
        private IApiMockerConfigRepository apiMockerConfigRepository;
        private readonly ILogger logger;        

        public CallsInterceptorMiddleware(RequestDelegate next, IApiMockerConfigRepository apiMockerConfigRepository, ILogger logger)
        {
            this.next = next;
            this.apiMockerConfigRepository = apiMockerConfigRepository;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopWatch = Stopwatch.StartNew();
            await this.next(context);
            
            var interceptedRoute = context.Request.Path;
            var apiMockerConfig = await apiMockerConfigRepository.GetConfig(ApiMockerCmdParams.ConfigFile);

            //search for the incoming request among the registered webservices
            var webServiceMock = apiMockerConfig.GetServiceMock(interceptedRoute);

            //if the route is registered and the http verb matches the json entry
            if (webServiceMock != null && string.Equals(webServiceMock.Verb, context.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                var content = await this.apiMockerConfigRepository.GetMockedResponse(webServiceMock.MockFile, apiMockerConfig.MocksFolder);
                context.Response.ContentType = webServiceMock.ContentType;
                context.Response.StatusCode = webServiceMock.HttpStatus;
                
                await context.Response.WriteAsync(content);

                stopWatch.Stop();
                logger.LogInformation($"Found : {interceptedRoute} - {stopWatch.Elapsed.Milliseconds}ms");
            }
            else
            {
                stopWatch.Stop();
                //exclude favico from logging
                if (!string.Equals(interceptedRoute, "/favicon.ico", StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogInformation($"Not found : {interceptedRoute}");
                }                
            }

            // Call the next delegate/middleware in the pipeline
            //await this.next(context);
        }
    }
}
