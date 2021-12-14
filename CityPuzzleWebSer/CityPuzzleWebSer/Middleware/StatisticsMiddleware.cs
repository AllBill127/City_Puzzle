using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Serilog;
using System.Diagnostics;

namespace CityPuzzleWebSer.WebApi.Middleware
{
    public class StatisticsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public StatisticsMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Starts the timer 
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Check what controller is being invoked
            var controllerActionDescriptor =
                context
                .GetEndpoint()
                .Metadata
                .GetMetadata<ControllerActionDescriptor>(); //Metadata of taken controller

            var controllerName = controllerActionDescriptor.ControllerName;
            var actionName = controllerActionDescriptor.ActionName;
            //Invoke next middleware
            await _next(context);
            //Stops the timer
            sw.Stop();
            //Logs into logging file
            _logger.Information($"It took {sw.ElapsedMilliseconds} ms to perform " +
                $"this action {actionName} in this controller {controllerName}");
        }
    }
}
