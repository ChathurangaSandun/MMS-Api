using Accoon.MMS.Api.Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Presenter.Middlewares
{
    public static class ExceptionMiddlewareExtensions
    {

        // TODO : exception factory
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    // create problemDetails
                    var problemDetails = new ProblemDetails();

                    if (exception is BadHttpRequestException badHttpRequestException)
                    {
                        problemDetails.Title = "Invalid request";
                        problemDetails.Status = (int)typeof(BadHttpRequestException).GetProperty("StatusCode",
                            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(badHttpRequestException);
                        problemDetails.Detail = badHttpRequestException.Message;
                    }
                    else if (exception is UnauthorizedAccessException unauthorizedAccessException)
                    {
                        problemDetails.Title = "Unauthorized error occurred";
                        problemDetails.Status = StatusCodes.Status401Unauthorized;
                        problemDetails.Detail = exception.Message.ToString();
                    }
                    else
                    {
                        problemDetails.Title = "An unexpected error occurred!";
                        problemDetails.Status = StatusCodes.Status500InternalServerError;
                        problemDetails.Detail = exception.Message.ToString();
                    }

                    problemDetails.Extensions["traceId"] = context.Request.HttpContext.TraceIdentifier;
                    if (env.IsDevelopment())
                    {
                        problemDetails.Extensions["errorDescription"] = exception.StackTrace.ToString();
                    }

                    context.Response.StatusCode = problemDetails.Status.Value;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(problemDetails));
                });
            });
        }
    }
}
