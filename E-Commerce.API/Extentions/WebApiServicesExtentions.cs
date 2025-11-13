using Microsoft.OpenApi.Models;

public static class WepApiServicesExtensions
{
    public static IServiceCollection AddWepApiServices(this IServiceCollection services, IConfiguration _configuration)
    {
        services.AddControllers();

        var frontUrl = _configuration.GetSection("URLS")["FrontUrl"];

        services.AddCors(op =>
        {
            op.AddPolicy("CorsPolicy", b =>
            {
                b.AllowAnyHeader().AllowAnyMethod()
                 .WithOrigins(frontUrl)
                 .AllowCredentials();
            });
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(op =>
        {
            op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                Description = "Enter 'Bearer' followed by space and your token"
            });

            op.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new string[] { }
                }
            });
        });

        // يجب أن تكون هنا (خارج swagger)
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
        });

        return services;
    }
}
