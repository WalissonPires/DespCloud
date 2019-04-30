using Base.WebApp.Contracts.Network;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.WebApp.Extensions;
using Newtonsoft.Json.Serialization;

namespace WebApi.StartupConfig
{
    public class TryCatchMiddleware
    {
        private readonly RequestDelegate _next;        

        public TryCatchMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILoggerFactory loggerFactory)
        {
            bool handleException = true; // context.Request.Headers["X-Requested-With"] == "XMLHttpRequest"; //ajax only

            if (handleException)
            {
                var logger = loggerFactory.CreateLogger(nameof(TryCatchMiddleware));

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    try
                    {
                        await HandleExceptionAsync(context, ex, logger);
                    }
                    catch (Exception ex2)
                    {
                        logger.LogCritical(ex, "Catch exception fail - InnerException");
                        logger.LogCritical(ex2, "Catch exception fail - OuterException");

                        throw ex;
                    }
                }
            }
            else
            {
                await _next(context);
            }

        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            var response = new ResponseError()
            {
                TraceId = null,
                Status = 500,
                Title = exception.GetRelevantMessage()
            };

            var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);

            logger.LogError(exception, "Ops");
        }
    }
}
