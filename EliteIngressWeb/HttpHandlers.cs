using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace EliteIngressWeb
{
    public static class HttpHandlers
    {
        internal static async Task PutJson(HttpContext context)
        {

            using (var streamReader = new StreamReader(context.Request.Body))
            {
                var body = await streamReader.ReadToEndAsync();
                Console.Out.WriteLine($"Received Json: {body}");

                //produce asynchronously for best throughput
                var deliveryReport = Program.Producer.ProduceAsync("commanders", null, body);
                deliveryReport.ContinueWith(task =>
                {
                    if (task.Result.Error.ToString() != "Success")
                    {
                        Console.WriteLine($"{task.Result.Error}: Partition: {task.Result.Partition}, Offset: {task.Result.Offset}");
                    }
                });
                //Console.WriteLine($"ERROR: {deliveryReport.Error} Partition: {deliveryReport.Partition}, Offset: {deliveryReport.Offset}");
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
