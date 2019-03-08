using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TVMaze.ScraperService.Service;

namespace TVMaze.ScraperService
{
    internal class Program
    {
        private static async Task Main(string [] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var scrappingBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ScrappingDataService>();
                });

            if (isService)
            {
                await scrappingBuilder.RunAsServiceAsync();
            }
            else
            {
                await scrappingBuilder.RunConsoleAsync();
            }
        }
    }
}
