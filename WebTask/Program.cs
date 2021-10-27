using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Azure.WebJobs.Extensions;
using Microsoft.Azure.WebJobs.Extensions.Storage;
// using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace WebTask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.ConfigureWebJobs(b => {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorage();
            });
            builder.ConfigureLogging((context, b) => {
                b.AddConsole();
                string key = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                b.AddApplicationInsightsWebJobs(options => {
                    options.InstrumentationKey = key;
                });
            });
            var host = builder.Build();
            using(host)
            {
                await host.RunAsync();
            }
        }
    }
}
