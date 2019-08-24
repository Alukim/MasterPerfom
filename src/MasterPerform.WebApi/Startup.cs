using MasterPerform.Contracts.Commands;
using MasterPerform.Infrastructure.Bootstrap;
using MasterPerform.Infrastructure.Elasticsearch;
using MasterPerform.Infrastructure.EnvironmentPrefixer;
using MasterPerform.Infrastructure.Messaging;
using MasterPerform.Infrastructure.Messaging.ServiceBus;
using MasterPerform.Infrastructure.Swagger;
using MasterPerform.Infrastructure.WebApi;
using MasterPerform.Infrastructure.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterPerform.WebApi
{
    /// <summary>
    /// WebApi startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="configuration">Configuration manager.</param>
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        /// <summary>
        /// Configuration manager.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Method allow to configure DI services.
        /// </summary>
        /// <param name="services">DI services container.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .RegisterCommandQueryProvider()
                .AddElasticSearchConnection(configuration: Configuration, defaultIndexName: MasterPerformBootstrap.IndexName)
                .RegisterEnvironmnentPrefixer()
                .RegisterEnvironmentSettings(Configuration)
                .RegisterServiceBusSettings(Configuration)
                .AddServiceBusSender()
                .AddMicroserviceBootstrap<MasterPerformBootstrap>()
                .AddContextAccessors()
                .AddSwaggerWithDocumentationFromAssemblyContaining<CreateDocument>()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        /// <summary>
        /// Method allow to configure application (hosting, logger, etc.)
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Host.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>()
                .UseMvc()
                .UseSwaggerCore()
                .ApplicationServices.UseModule();
        }
    }
}
