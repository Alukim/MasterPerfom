using MasterPerform.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.Swagger;

namespace MasterPerform.WebApi.Utilities.Swagger
{
    /// <summary>
    /// Implementation of swagger configuration.
    /// </summary>
    internal class SwaggerConfiguration : ISwaggerConfiguration
    {
        public Info SwaggerInfo => new Info
        {
            Version = "v1",
            Title = "Master Perform API",
            Description = "Simple API of Master of Science project",
            Contact = new Contact
            {
                Name = "Bartosz Kowalski",
                Email = "bartkow044@student.polsl.pl"
            }
        };

        public string SwaggerTitle => "Master Perform API V1 - Alpha - 1.0.0";
        public string SwaggerVersion => "v1";
    }
}
