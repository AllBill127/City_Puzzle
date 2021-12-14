using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.AspNetCore;

namespace CityPuzzleWebSer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            Log.Logger = new LoggerConfiguration()
                                            .WriteTo.File("Log_{Date}.txt")
                                            .MinimumLevel.Debug()
                                            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                                            .Enrich.FromLogContext()
                                            .CreateLogger();

            CreateHostBuilder(args).Build().Run();

            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                   Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>();
                       }).UseSerilog();

    }
}