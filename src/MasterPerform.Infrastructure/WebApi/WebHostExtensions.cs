using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.IO;

namespace MasterPerform.Infrastructure.WebApi
{
    public static class WebHostExtensions
    {
        public static void BuildAndRunWebHost<TStartup>() where TStartup : class
        {
            CreateLogger();

            if (BuildConstants.IsDebug)
            {
                Console.WriteLine("Process ID: " + Process.GetCurrentProcess().Id);
            }

            try
            {
                BuildWebHost<TStartup>().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly: {0}", ex.Message);
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost<TStartup>() where TStartup : class
            => CreatePreconfiguredWebHostBuilder<TStartup>().Build();

        public static IWebHostBuilder CreatePreconfiguredWebHostBuilder<TStartup>() where TStartup : class
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(ConfigureConfigurationSources)
                .UseDefaultServiceProvider((context, options) => { options.ValidateScopes = BuildConstants.IsDebug; })
                .UseSerilog()
                .UseStartup<TStartup>();
        }

        private static void ConfigureConfigurationSources(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var env = hostingContext.HostingEnvironment.EnvironmentName.ToLowerInvariant();

            config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }

        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole(
                    LogEventLevel.Debug,
                    "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
