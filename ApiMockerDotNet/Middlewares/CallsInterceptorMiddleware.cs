using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ApiMockerDotNet.Repositories;

namespace ApiMockerDotNet.Middlewares
{
    public class CallsInterceptorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiMockerConfigRepository _apiMockerConfigRepository;
        private readonly ILogger<CallsInterceptorMiddleware> _logger;        

        public CallsInterceptorMiddleware(RequestDelegate next, IApiMockerConfigRepository apiMockerConfigRepository, ILogger<CallsInterceptorMiddleware> logger)
        {
            _next = next;
            _apiMockerConfigRepository = apiMockerConfigRepository;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopWatch = Stopwatch.StartNew();
            await _next(context);
            
            var interceptedRoute = context.Request.Path;
            var apiMockerConfig = await _apiMockerConfigRepository.GetConfig(ApiMockerCmdParams.ConfigFile);

            //search for the incoming request among the registered webservices
            var webServiceMock = apiMockerConfig.GetServiceMock(interceptedRoute);

            //if the route is registered and the http verb matches the json entry
            if (webServiceMock != null && string.Equals(webServiceMock.Verb, context.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                var content = await _apiMockerConfigRepository.GetMockedResponse(webServiceMock.MockFile, apiMockerConfig.MocksFolder);
                context.Response.ContentType = webServiceMock.ContentType;
                context.Response.StatusCode = webServiceMock.HttpStatus;
                
                await context.Response.WriteAsync(content);

                stopWatch.Stop();
                _logger.LogInformation($"Found : {interceptedRoute} - {stopWatch.Elapsed.Milliseconds}ms");
            }
            else
            {
                stopWatch.Stop();
                //exclude favico from logging
                if (!string.Equals(interceptedRoute, "/favicon.ico", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation($"Not found : {interceptedRoute}");
                }                
            }

            // Call the next delegate/middleware in the pipeline
            //await this.next(context);
        }
    }
}
