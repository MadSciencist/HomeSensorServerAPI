using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace HomeSensorServerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                AppDbContext context = services.GetRequiredService<AppDbContext>();

                try
                {
                    DbSeeder seeder = new DbSeeder();
                    seeder.EnsurePopulated(context);
                }
                catch (Exception ex)
                {
                    //TODO proper exception logging
                    Console.WriteLine(ex.Message);
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
               .UseKestrel(options => options.Listen(IPAddress.Any, 80));
    }
}