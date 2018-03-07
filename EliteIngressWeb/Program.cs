using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka;
using System;
using System.IO;
using System.Collections.Generic;
using Confluent.Kafka.Serialization;
using System.Text;

namespace EliteIngressWeb
{
    class Program
    {
        internal static Producer<Null, string> Producer;

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


                var kafkaConfig = new Dictionary<string, object> {
                        { "bootstrap.servers", "localhost:9092" }, //where to connect
                        { "socket.keepalive.enable", true},
                        { "queue.buffering.max.ms", 1}, // How long to buffer messages locally before flushing them to the broker
                    };

                using (Producer = new Producer<Null, string>(kafkaConfig, null, new StringSerializer(Encoding.UTF8)))
                {

                    Console.WriteLine($"Kafka producer {Producer.Name} running...");
                    host.Run(); //this blocks untill close time

                    Producer.Flush(TimeSpan.FromSeconds(10)); //we might have messages in the air, clear them...
                }

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
