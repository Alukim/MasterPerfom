using MasterPerform.Infrastructure.Messaging.Contracts;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace MasterPerform.Infrastructure.Swagger.Filters
{
    internal class CreatedIdParameterFilter : IOperationFilter
    {
        private const string CreatedIdFieldParameter = "createdId";
        private static readonly Type CreateCommandType = typeof(ICreateCommand);

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.ActionDescriptor.Parameters.Any(p =>
                CreateCommandType.IsAssignableFrom(p.ParameterType)))
                return;

            var reference = (operation.Parameters.Single(t => t is BodyParameter) as BodyParameter)?.Schema.Ref;

            if (string.IsNullOrEmpty(reference))
                return;

            var definition = context.SchemaRegistry.Definitions[reference.Substring(reference.LastIndexOf('/') + 1)];

            if (definition.Properties != null && definition.Properties.Any())
                definition.Properties.Remove(CreatedIdFieldParameter);
        }
    }
}
