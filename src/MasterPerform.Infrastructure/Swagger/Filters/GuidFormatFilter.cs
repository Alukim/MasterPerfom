using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace MasterPerform.Infrastructure.Swagger.Filters
{
    internal class GuidFormatFilter : ISchemaFilter
    {
        private const string GuidSwaggerFormat = "uuid";

        public void Apply(Schema schema, SchemaFilterContext context)
        {
            if (schema.Properties is null)
                return;

            foreach (var schemaProperty in schema.Properties)
            {
                if (schemaProperty.Value.Format == GuidSwaggerFormat)
                    schemaProperty.Value.Example = Guid.NewGuid();
            }
        }
    }
}
