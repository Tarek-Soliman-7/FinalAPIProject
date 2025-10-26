using Domain.Contracts;
using E_Commerce.API.Factories;
using Microsoft.AspNetCore.Mvc;
using Presistence.Data;
using Presistence.Repositories;

namespace E_Commerce.API.Extensions
{
    public static class WepApiServicesExtensions
    {
        public static IServiceCollection AddWepApiServices(this IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });

            

            return services;
        }

    }
}
