
using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;

namespace E_Commerce.API.Extensions
{
    public static class CoreServicesExtentions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddAutoMapper(x => { }, typeof(AssemblyReference).Assembly);
            services.AddScoped<IServiceManger, ServiceManger>();

            return services;
        }
    }
}
