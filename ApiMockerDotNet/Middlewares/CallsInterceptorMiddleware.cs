using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ApiMockerDotNet.Entities;
using ApiMockerDotNet.Repositories;

namespace ApiMockerDotNet.Middlewares
{
    public class CallsInterceptorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IApiMockerConfigRepository apiMockerConfigRepository;
        private readonly ILogger<CallsInterceptorMiddleware> logger;
        private ApiMockerConfig apiMockerConfig;

        public CallsInterceptorMiddleware(RequestDelegate next, IApiMockerConfigRepository apiMockerConfigRepository, ILogger<CallsInterceptorMiddleware> logger)
        {
            this.next = next;
            this.apiMockerConfigRepository = apiMockerConfigRepository;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline
            //await _next(context);

            var stopWatch = Stopwatch.StartNew();

            var interceptedRoute = context.Request.Path;
            apiMockerConfig = await apiMockerConfigRepository.GetConfig(ApiMockerCmdParams.ConfigFile);
                                   
            //search for the incoming request among the registered webservices
            var webServiceMock = apiMockerConfig.GetServiceMockByUrl(interceptedRoute);

            //if the route is registered and the http verb matches the json entry
            if (webServiceMock != null && string.Equals(webServiceMock.Verb, context.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                var content = await apiMockerConfigRepository.GetMockedResponse(webServiceMock.MockFile, apiMockerConfig.MocksFolder);
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
                    logger.LogInformation($"Not found : {interceptedRoute} - {stopWatch.Elapsed.Milliseconds}ms");
                }                
            }            
        }
    }
}
