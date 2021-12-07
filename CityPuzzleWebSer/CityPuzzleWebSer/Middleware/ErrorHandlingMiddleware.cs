using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Http.Extensions;

namespace MiddlewareExamples.WebApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        //To show stack trace (for development)
        private readonly IHostingEnvironment _environment;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger, IHostingEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Invoke next middleware
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogErrorExceptionWithRequestBody(context, ex); //Logs exception
                await HandleExceptionAsync(context, ex); //Handles exception
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorMessage = "Internal server error happened. Please contact support";
            //The whole stack message (for development)
            if (_environment.IsDevelopment())
            {
                errorMessage = JsonConvert.SerializeObject(exception, Formatting.Indented);
            }

            return context.Response.WriteAsync(new
            {
                Message = errorMessage
            }.ToString());
        }

        private async Task LogErrorExceptionWithRequestBody(HttpContext context, Exception exception)
        {
            context.Request.EnableBuffering();
            context.Request.Body.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(context.Request.Body))
            {
                //Gets the body that was passed into the request
                var body = await reader.ReadToEndAsync();
                //Log everything about the exception
                _logger.Error(
                    exception,
                    $"WebApi exception, Method: {{method}}, Content: {{faultMessage}}",
                    $"{context.Request.Method} {context.Request.GetDisplayUrl()}",
                    JsonConvert.SerializeObject(body));

                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}
