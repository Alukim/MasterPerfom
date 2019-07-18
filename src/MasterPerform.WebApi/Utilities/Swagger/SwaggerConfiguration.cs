using MasterPerform.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.Swagger;

namespace MasterPerform.WebApi.Utilities.Swagger
{
    internal class SwaggerConfiguration : ISwaggerConfiguration
    {
        public Info SwaggerInfo => new Info
        {
            Version = "v1-alpha-1.0.0",
            Title = "Master Perform API",
            Description = "Simple API of Master Perform semestral project",
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
