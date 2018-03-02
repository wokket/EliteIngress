using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace EliteIngressWeb
{
    class Program
    {
        static int Main(string[] args)
        {

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
               .UseUrls("http://*:5000")
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
                Console.WriteLine(ex.ToString());
                return 1;
            }
        }

    }
}
