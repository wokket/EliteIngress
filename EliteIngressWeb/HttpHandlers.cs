using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EliteIngressWeb
{
    public static class HttpHandlers
    {
        internal static async Task PutJson(HttpContext context)
        {

            using (var streamReader = new StreamReader(context.Request.Body)) {
                var body = await streamReader.ReadToEndAsync();
                Program.Log.Debug("Received Json: {message}", body);
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
