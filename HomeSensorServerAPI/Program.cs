using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace HomeSensorServerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                var host = CreateWebHostBuilder(args).Build();
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    IServiceProvider services = scope.ServiceProvider;
                    AppDbContext context = services.GetRequiredService<AppDbContext>();

                    try
                    {
                        DbSeeder seeder = new DbSeeder();
                        //seeder.UpdateDatabase(context);
                        seeder.EnsurePopulated(context);
                    }
                    catch (Exception e)
                    {
                        logger.Error(e);
                    }
                }
                host.Run();
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logger =>
                {
                    //logger.ClearProviders();
                    logger.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .UseKestrel(options => options.Listen(IPAddress.Any, 80));
    }
}