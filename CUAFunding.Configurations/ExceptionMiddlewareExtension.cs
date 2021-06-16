using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace CUAFunding.Configurations
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;
                        var path = context.Request.Path;
                        var message = string.Empty;
                        var result = string.Empty;
                        dynamic response = null;

                        if (env.EnvironmentName == "Test" || env.IsDevelopment())
                        {
                            response = new
                            {
                                StatusCode = context.Response.StatusCode,
                                ExMessage = message,
                                response,
                                exception
                            };
                        }
                        else
                        {
                            response = new
                            {
                                StatusCode = context.Response.StatusCode,
                                ExMessage = message,
                                response
                            };
                        }
                        await BaseExceptionResponce(context.Response, response);
                    }
                });
            });
        }

        private static async Task BaseExceptionResponce(Microsoft.AspNetCore.Http.HttpResponse httpResponce, object responce)
        {
            var result = JsonConvert.SerializeObject(responce);
            await httpResponce.WriteAsync(result);
        }
    }

}
