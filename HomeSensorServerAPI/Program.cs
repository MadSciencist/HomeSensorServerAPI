using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Net;

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

                        logger.Info("Applying migrations to the DB");
                        seeder.UpdateDatabase(context);

                        logger.Info("Ensuring that DB has initial seed");
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
                logger.Fatal(e);
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
                    logger.SetMinimumLevel(LogLevel.Information);
                })
                .UseNLog()
                .UseKestrel(options =>
                {
                    options.Limits.MaxConcurrentConnections = 100;
                    options.Limits.MaxConcurrentUpgradedConnections = 100;
                    options.Listen(IPAddress.Any, 5000);
                });
    }
}