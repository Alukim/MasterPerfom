using Swashbuckle.AspNetCore.Swagger;

namespace MasterPerform.Infrastructure.Swagger
{
    public interface ISwaggerConfiguration
    {
        Info SwaggerInfo { get; }
        string SwaggerVersion { get; }
        string SwaggerTitle { get; }
    }
}
