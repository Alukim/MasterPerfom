using MasterPerform.Infrastructure.Bootstrap;
using MasterPerform.Infrastructure.ElasticSearch;
using MasterPerform.WebApi.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterPerform.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddElasticSearchConnection(configuration: Configuration, defaultIndexName: MasterPerformBootstrap.IndexName)
                .AddMicroserviceBootstrap<MasterPerformBootstrap>()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            app.ApplicationServices.UseModule();
        }
    }
}
