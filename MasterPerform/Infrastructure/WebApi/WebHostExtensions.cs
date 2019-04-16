using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;

namespace MasterPerform.Infrastructure.WebApi
{
    public static class WebHostExtensions
    {
        public static void BuildAndRunWebHost<TStartup>() where TStartup : class
        {
            // Initialize startup logger, to log errors

            if (BuildConstants.IsDebug)
            {
                Console.WriteLine("Process ID: " + Process.GetCurrentProcess().Id);
            }

            try
            {
                BuildWebHost<TStartup>();
            }
            catch (Exception ex)
            {
                // Add Logger
                Console.WriteLine($"Host terminated unexpectedly: {ex.Message}");
                throw;
            }
            finally
            {
                // flush logger
            }
        }

        public static IWebHost BuildWebHost<TStartup>() where TStartup : class =>
            CreatePreconfiguredWebHostBuilder<TStartup>().Build();

        public static IWebHostBuilder CreatePreconfiguredWebHostBuilder<TStartup>() where TStartup : class
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(ConfigureConfigurationSources)
                .UseDefaultServiceProvider((context, options) => { options.ValidateScopes = true; })
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
    }
}
