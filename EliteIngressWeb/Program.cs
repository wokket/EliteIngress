using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace EliteIngressWeb
{
    class Program
    {
        internal static ILogger Log;

        static int Main(string[] args)
        {
            Log = new LoggerConfiguration()
                       .WriteTo.Console()
                       .MinimumLevel.Debug()
                       .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                       .Enrich.FromLogContext()
                       .CreateLogger();

            Serilog.Log.Logger = Log;

            try
            {
                // see https://www.strathweb.com/2017/01/building-microservices-with-asp-net-core-without-mvc/

                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables().Build();

                var host = WebHost.CreateDefaultBuilder(args)
               .UseKestrel()
               .UseConfiguration(config)
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseUrls("http://*:9008")
               .UseSerilog()
               .ConfigureServices(s => s.AddRouting())
               .Configure(app =>
               {
                   app.UseRouter(r =>
                   {
                       // root of website
                       r.MapPut("", HttpHandlers.PutJson);
                       r.MapGet("", HttpHandlers.GetRoot);
                   });
               })
               .Build();


                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Serilog.Log.CloseAndFlush();
            }
        }

    }
}
