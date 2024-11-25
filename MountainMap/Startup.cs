using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(MountainMap.Startup))]
namespace MountainMap
{
    public class Startup : IHostingStartup
    {

        public void Configure(IWebHostBuilder builder)
        {

            var services = builder.ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                


                

            });


        }

    }
}
