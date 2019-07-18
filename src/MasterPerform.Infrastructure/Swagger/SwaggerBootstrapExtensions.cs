using MasterPerform.Infrastructure.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace MasterPerform.Infrastructure.Swagger
{
    public static class SwaggerBootstrapExtensions
    {
        private static readonly Type SwaggerConfigurationType = typeof(ISwaggerConfiguration);
        private static ISwaggerConfiguration swaggerConfiguration;

        public static IServiceCollection AddSwaggerWithDocumentationFromAssemblyContaining<TType>(this IServiceCollection serviceCollection)
        {
            var assemblies = new List<Assembly> { typeof(TType).GetTypeInfo().Assembly };
            var entryAssembly = Assembly.GetEntryAssembly();

            if (!assemblies.Contains(entryAssembly))
                assemblies.Add(entryAssembly);

            var xmlFileList = assemblies.Select(z => z.ManifestModule.Name.Replace("dll", "xml")).ToList();

            serviceCollection.AddSwaggerGen(c =>
            {
                c.ConfigureSwaggerDoc(entryAssembly);
                c.AddXmlDocumentation(Path.GetDirectoryName(entryAssembly.Location), xmlFileList);
                c.OperationFilter<CreatedIdParameterFilter>();
                c.SchemaFilter<GuidFormatFilter>();
            });

            return serviceCollection;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(z =>
            {
                z.EnableFilter();
                z.EnableDeepLinking();
                z.DisplayRequestDuration();
                z.SwaggerEndpoint("/swagger/v1/swagger.json", swaggerConfiguration?.SwaggerTitle ?? "API");
            });

            return app;
        }

        private static void ConfigureSwaggerDoc(this SwaggerGenOptions options, Assembly entryAssembly)
        {
            var swaggerConfigurationType = entryAssembly
                .GetTypes()
                .FirstOrDefault(x => SwaggerConfigurationType.IsAssignableFrom(x));

            if (swaggerConfigurationType is null)
                return;

            swaggerConfiguration = (ISwaggerConfiguration) Activator.CreateInstance(swaggerConfigurationType);

            options.SwaggerDoc(swaggerConfiguration.SwaggerVersion, swaggerConfiguration.SwaggerInfo);
        }

        private static void AddXmlDocumentation(this SwaggerGenOptions options, string directory, IEnumerable<string> files)
        {
            XElement xml = null;

            foreach (var file in files)
            {
                var xmlFile = Path.Combine(directory, file);

                if (!File.Exists(xmlFile))
                    continue;

                var xElement = XElement.Load(xmlFile);

                if (xml is null)
                    xml = xElement;
                else
                {
                    foreach (var descendant in xElement.Descendants())
                        xml.Add(descendant);
                }
            }

            if (xml is null)
                return;

            var swaggerDocumentationFile = Path.Combine(directory, "SwaggerDocumentation.xml");
            using (var stream = File.Create(swaggerDocumentationFile))
                xml.Save(stream);

            options.IncludeXmlComments(swaggerDocumentationFile);
        }
    }
}
