using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetMQ;

namespace EliteIngressWeb
{
    public static class HttpHandlers
    {
        internal static async Task PutJson(HttpContext context)
        {

            using (var streamReader = new StreamReader(context.Request.Body)) {
                var body = await streamReader.ReadToEndAsync();
                Console.WriteLine($"Received Json: {body}");

                Program.Publisher.SendMoreFrame("").SendFrame(body);

            }

            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("ok");
        }

        internal static Task GetRoot(HttpContext context)
        {
            context.Response.StatusCode = 200;
            return context.Response.WriteAsync("ok");
        }
    }
}
