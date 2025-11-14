
using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;
using Shared.Common;

namespace E_Commerce.API.Extensions
{
    public static class CoreServicesExtentions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAutoMapper(x => { }, typeof(AssemblyReference).Assembly);
            services.AddScoped<IServiceManger, ServiceManger>();

            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));//IOptions

            return services;
        }
    }
}
